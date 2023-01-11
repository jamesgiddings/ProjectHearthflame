using GramophoneUtils.Stats;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using GramophoneUtils.Characters;

public class AnimationPlayer
{
	private Animator animator;
    private Battler battler;
    private Character character;

    private CharacterGameObjectManager _characterGameObjectManager;

    private string attack;
    private string idle;
    private string walkToAttack;
    private string walkFromAttack;
    private string cast;
    private string shoot;

    public AnimationPlayer(Battler battler, Character character)
	{
        this.battler = battler;
        this.character = character;
        Debug.Log(this.character.Name + " is getting a new Animation player");
        _characterGameObjectManager = ServiceLocator.Instance.CharacterGameObjectManager;
        animator = battler.gameObject.GetComponent<Animator>();

        SetAnimationDirections();

        Debug.Log(character.Name + " animator: " + (animator == null));
        Debug.Log(character.Name + "'s animControllerPath: " + character.AnimControllerPath);
        if (!character.IsPlayer)
        {
            RuntimeAnimatorController runtimeAnimatorController = Resources.Load(character.AnimControllerLoadPath) as RuntimeAnimatorController;
            animator.runtimeAnimatorController = runtimeAnimatorController;
        }

        
        //Debug.Log(character.CharacterTemplate.AnimControllerPath);
        //Debug.Log(character.Name + " runtimeAnimatorController: " + (animator.runtimeAnimatorController == null));
        animator.enabled = true;
        //Debug.Log(character.Name);

        animator.Play(idle);
    }

	private void SetAnimationDirections()
	{
        if (character.IsPlayer)
        {
            attack = "Attack_Right";
            idle = "Idle_Right";
            walkToAttack = "Walk_Right";
            walkFromAttack = "Walk_Left";
            cast = "Cast_Right";
            shoot = "Shoot_Right";
        }
		else
		{
            attack = "Attack_Left";
            idle = "Idle_Left";
            walkToAttack = "Walk_Left";
            walkFromAttack = "Walk_Right";
            cast = "Cast_Left";
            shoot = "Cast_Left";
        }
    }

	public void DisplayAnimation(ISkill skill, List<Character> targets)
    {
        switch (skill.SkillAnimType)
        {
            case (SkillAnimType.MeleePhysical):
                Sequence sequence = DOTween.Sequence();
                Vector3 startPos = battler.gameObject.transform.position;
                foreach (Character target in targets)
                {
                    Vector3 currentPos = battler.gameObject.transform.position;
                    Vector3 targetPos = Vector3.Lerp(battler.gameObject.transform.position, _characterGameObjectManager.CharacterBattlerDictionary[target].transform.position, 0.65f);
                    sequence.AppendCallback(() => animator.Play(walkToAttack));
                    sequence.Append(battler.gameObject.transform.DOMove(targetPos, 0.4f));
                    sequence.AppendCallback(() => animator.Play(attack));
                    
                }
                sequence.Append(battler.gameObject.transform.DOShakeRotation(0.5f, 2).OnComplete(() => skill.DoNextBit(targets, character)));
                sequence.AppendCallback(() => animator.Play(walkFromAttack));
                sequence.Append(battler.gameObject.transform.DOMove(startPos, 0.4f));
                sequence.AppendCallback(() => animator.Play(idle));
                sequence.AppendInterval(0.5f).WaitForCompletion();
                sequence.Play().OnComplete(() => battler.OnTurnComplete?.Invoke());
                break;
            case (SkillAnimType.MagicalProjectile):
                Sequence projectileSequence = DOTween.Sequence();

                List<GameObject> projectiles = new List<GameObject>();

                projectileSequence.AppendCallback(() => animator.Play(cast));
                projectileSequence.AppendInterval(0.8f).WaitForCompletion();
                projectileSequence.AppendCallback(() => animator.Play(idle));
                projectileSequence.AppendInterval(0.05f).WaitForCompletion();

                foreach (Character target in targets)
                {
                    GameObject projectile = Object.Instantiate(skill.ProjectilePrefab, battler.gameObject.transform);
                    Animator projectileAnimator = projectile.GetComponent<Animator>();
                    projectileAnimator.runtimeAnimatorController = skill.AnimatorController;
                    projectile.transform.localPosition = new Vector3(0, 0, 0);
                    projectile.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

                    Vector3 targetPos = Vector3.Lerp(battler.gameObject.transform.position, _characterGameObjectManager.CharacterBattlerDictionary[target].transform.position, 0.85f);

                    Vector2 dir = targetPos - battler.transform.position;

                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                    projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                    projectiles.Add(projectile);
                    projectileSequence.Join(projectile.transform.DOMove(targetPos, 0.6f));
                    projectileSequence.Join(projectile.transform.DOScale(3f, 0.6f));
                }

                projectileSequence.Play().OnComplete(() =>
                {
                    DestroyProjectiles(projectiles);
                    skill.DoNextBit(targets, character);
                    animator.Play(idle);
                    battler.OnTurnComplete?.Invoke();
                });

                break;

            case (SkillAnimType.PhysicalProjectile):
                Sequence physicalProjectileSequence = DOTween.Sequence();

                List<GameObject> physicalProjectiles = new List<GameObject>();

                physicalProjectileSequence.AppendCallback(() => animator.Play(shoot));
                physicalProjectileSequence.AppendInterval(1f).WaitForCompletion();
                physicalProjectileSequence.AppendCallback(() => animator.Play(idle));
                physicalProjectileSequence.AppendInterval(0.05f).WaitForCompletion();

                foreach (Character target in targets)
                {
                    GameObject physicalProjectile = Object.Instantiate(skill.ProjectilePrefab, battler.gameObject.transform);
                    Animator pysicalProjectileAnimator = physicalProjectile.GetComponent<Animator>();
                    pysicalProjectileAnimator.runtimeAnimatorController = skill.AnimatorController;
                    physicalProjectile.transform.localPosition = new Vector3(0, 0, 0);
                    //physicalProjectile.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

                    Vector3 targetPos = Vector3.Lerp(battler.gameObject.transform.position, _characterGameObjectManager.CharacterBattlerDictionary[target].transform.position, 0.85f);

                    Vector2 dir = targetPos - battler.transform.position;

                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                    physicalProjectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                    physicalProjectiles.Add(physicalProjectile);
                    physicalProjectileSequence.Join(physicalProjectile.transform.DOMove(targetPos, 0.6f));
                }

                physicalProjectileSequence.Play().OnComplete(() =>
                {
                    DestroyProjectiles(physicalProjectiles);
                    skill.DoNextBit(targets, character);
                    animator.Play(idle);
                    battler.OnTurnComplete?.Invoke();
                });

                break;
            case (SkillAnimType.BuffOrDebuff):
                foreach (Character target in targets)
                {
                    GameObject effect = Object.Instantiate(skill.EffectPrefab, battler.gameObject.transform);
                    Animator effectAnimator = effect.GetComponent<Animator>();
                    effectAnimator.runtimeAnimatorController = skill.AnimatorController;

                    SpriteRenderer effectSpriteRenderer = effect.GetComponent<SpriteRenderer>();

                    Sequence buffDebuffSequence = DOTween.Sequence();
                    buffDebuffSequence.AppendCallback(() => animator.Play(cast));
                    buffDebuffSequence.AppendInterval(0.8f).WaitForCompletion();
                    buffDebuffSequence.AppendCallback(() => animator.Play(idle));
                    buffDebuffSequence.AppendInterval(0.1f).WaitForCompletion();
                    buffDebuffSequence.AppendCallback(() => skill.DoNextBit(targets, character));

                    buffDebuffSequence.Insert(0.5f, effectSpriteRenderer.DOFade(0f, 0.8f).From());
                    buffDebuffSequence.AppendInterval(0.1f);
                    buffDebuffSequence.Append(effectSpriteRenderer.DOFade(0f, 0.4f));

                    buffDebuffSequence.Play().OnComplete(
                        () =>
                        {
                            animator.Play(idle);
                            battler.OnTurnComplete?.Invoke();
                        }
                        );
                   }
                break;
        }
    }

    private void DestroyProjectiles(List<GameObject> projectiles)
    {
        foreach (GameObject projectile in projectiles)
        {
            Object.Destroy(projectile);
        }
    }
}
