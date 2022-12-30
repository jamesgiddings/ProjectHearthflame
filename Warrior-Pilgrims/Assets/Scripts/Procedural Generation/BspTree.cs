using UnityEngine;
public class BspTree
{
    public RectInt container;
    public RectInt room;
    public BspTree left;
    public BspTree right;

    public BspTree(RectInt a)
    {
        container = a;
    }

    internal static BspTree Split(int numberOfOperations, RectInt container)
    {
        var node = new BspTree(container);

        if (numberOfOperations == 0)
        {
            return node;
        }

        RectInt[] splitedContainer = SplitContainer(container);
        node.left = Split(numberOfOperations - 1, splitedContainer[0]);
        Debug.Log(numberOfOperations);

        node.right = Split(numberOfOperations - 1, splitedContainer[1]);
        Debug.Log(numberOfOperations);

        return node;
    }

    private static RectInt[] SplitContainer(RectInt container)
    {
        RectInt c1, c2;
        bool horizontal = Random.Range(0f, 1f) > 0.5f ? true : false;
        if (horizontal)
        {
            c1 = new RectInt(container.x, container.y, (int)(container.width * Random.Range(0.3f, 0.6f)), container.height);
            c2 = new RectInt(container.x + c1.width, container.y, container.width - c1.width, container.height);
        }
        else
        {
            c1 = new RectInt(container.x, container.y, container.width, (int)(container.height * Random.Range(0.3f, 0.6f)));
            c2 = new RectInt(container.x, container.y + c1.height, container.width, container.height - c1.height);
        }
        return new RectInt[] { c1, c2 };
    }
}

//public class Rectangle
//{

//    private static int MIN_SIZE = 5;

//    private int top, left, width, height;
//    private Rectangle leftChild;
//    private Rectangle rightChild;
//    private Rectangle dungeon;

//    public Rectangle(int top, int left, int height, int width)
//    {
//        this.top = top;
//        this.left = left;
//        this.width = width;
//        this.height = height;
//    }

//    public bool split()
//    {
//        if (leftChild != null) //if already split, bail out
//            return false;
//        bool horizontal = rnd.nextBoolean(); //direction of split
//        int max = (horizontal ? height : width) - MIN_SIZE; //maximum height/width we can split off
//        if (max <= MIN_SIZE) // area too small to split, bail out
//            return false;
//        int split = rnd.nextInt(max); // generate split point 
//        if (split < MIN_SIZE)  // adjust split point so there's at least MIN_SIZE in both partitions
//            split = MIN_SIZE;
//        if (horizontal)
//        { //populate child areas
//            leftChild = new Rectangle(top, left, split, width);
//            rightChild = new Rectangle(top + split, left, height - split, width);
//        }
//        else
//        {
//            leftChild = new Rectangle(top, left, height, split);
//            rightChild = new Rectangle(top, left + split, height, width - split);
//        }
//        return true; //split successful
//    }

//    public void generateDungeon()
//    {
//        if (leftChild != null)
//        { //if current are has child areas, propagate the call
//            leftChild.generateDungeon();
//            rightChild.generateDungeon();
//        }
//        else
//        { // if leaf node, create a dungeon within the minimum size constraints
//            int dungeonTop = (height - MIN_SIZE <= 0) ? 0 : rnd.nextInt(height - MIN_SIZE);
//            int dungeonLeft = (width - MIN_SIZE <= 0) ? 0 : rnd.nextInt(width - MIN_SIZE);
//            int dungeonHeight = Mathf.Max(rnd.nextInt(height - dungeonTop), MIN_SIZE); ;
//            int dungeonWidth = Mathf.Max(rnd.nextInt(width - dungeonLeft), MIN_SIZE); ;
//            dungeon = new Rectangle(top + dungeonTop, left + dungeonLeft, dungeonHeight, dungeonWidth);
//        }
//    }

//}





























//    private Room[] Split(Room room)
//    {
//        Room[] rooms = new Room[2];

//        if ((room.roomWidth * room.splittingPosition) < dungeonMinRoomWidth)

//        // choose axis
//        if (Random.Range(0f, 1f) < room.axisSplitSkewToHorizontal)
//        {
//            rooms[0] = room;
//        }
//        else
//        {
//            // vertical
//        }
//            // choose split point

//        return rooms;
//    }
//}


//public class RoomBlueprint : ScriptableObject
//{
//    [SerializeField] int minRoomWidth = 0;
//    [SerializeField] int maxRoomWidth = 10;

//    [SerializeField] int minRoomHeight = 0;
//    [SerializeField] int maxRoomHeight = 10;

//    [SerializeField] float minSplittingPosition = 0.3f;
//    [SerializeField] float maxSplittingPosition = 0.7f;

//    [SerializeField] float axisSplitSkewToHorizontal = 0.5f;

//    public Room CreateRoomMaster()
//    {
//        return new Room(
//            Vector2.zero,
//            Random.Range(minRoomWidth, maxRoomWidth),
//            Random.Range(minRoomHeight, maxRoomHeight),
//            Random.Range(minSplittingPosition, maxSplittingPosition),
//            axisSplitSkewToHorizontal
//        );
//    }
//}

//public class Room
//{
//    public Room(Vector2 position, int roomWidth, int roomHeight, float splittingPosition, float axisSplitSkewToHorizontal)
//    {
//        this.position = position;
//        this.roomWidth = roomWidth;
//        this.roomHeight = roomHeight;
//        this.splittingPosition = splittingPosition;
//        this.axisSplitSkewToHorizontal = axisSplitSkewToHorizontal;
//    }

//    public Vector2 position;
//    public int roomWidth;
//    public int roomHeight;
//    public float splittingPosition;
//    public float axisSplitSkewToHorizontal;


//    public int RoomWidth => roomWidth;
//    public int RoomHeight => roomHeight;
//    public float SplittingPosition => splittingPosition;
//    public float AxisSplitSkewToHorizontal => axisSplitSkewToHorizontal;
//}

