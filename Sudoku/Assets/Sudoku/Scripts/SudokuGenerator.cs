using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SudokuGenerator : MonoBehaviour
{
    public int currentNumberValue = 0;
    public int clueWeight1 = 1;
    public int clueWeight2 = 4;
    public int maxCluesInRegion = 3;
    public int mutations = 1000;
    public bool useMutations = true;
    public bool useMaxClues = true;       
    public bool checkEmptyGroups = true;
    public bool hitNumber = false;
    public GameObject numberPrefab, borderPrefab, sudokuParent;
    public float spacing = 1;
    public bool limitCheck = false;
    public int checkLimit = 5;
    public string lasttilepressed = "";
    public string lastbuttonpressed = "";
    public string currentbuttonpressed = "";
    private bool check = false;
    private int _checkLimit = 0;
    private GameObject _sudokuParent;
    private List<GameObject> tiles = new List<GameObject>();
    private Ray ray;
    private RaycastHit hit;
    private int minutes = 0;
    private int seconds = 0;
    private int cycle = 0;
    [HideInInspector]
    public List<string> generates = new List<string>();
    [HideInInspector]
    public int repeats=0;

    // Don't change following variables!
    private int[,] field = new int[9, 9];
    private int[,] hfield = new int[1, 9];
    private int[,] _hfield = new int[1, 9];
    private int[,] vfield = new int[9, 1];
    private int[,] _vfield = new int[9, 1];
    private int currentMutation = 0;
    private int rndNum = 0, _rndNum = 0, rangeMin, rangeMax;
    private int[] chunk1 = new int[9] { 0, 1, 2, 9, 10, 11, 18, 19, 20 };
    private int[] chunk2 = new int[9] { 3, 4, 5, 12, 13, 14, 21, 22, 23 };
    private int[] chunk3 = new int[9] { 6, 7, 8, 15, 16, 17, 24, 25, 26 };
    private int[] chunk4 = new int[9] { 27, 28, 29, 36, 37, 38, 45, 46, 47 };
    private int[] chunk5 = new int[9] { 30, 31, 32, 39, 40, 41, 48, 49, 50 };
    private int[] chunk6 = new int[9] { 33, 34, 35, 42, 43, 44, 51, 52, 53 };
    private int[] chunk7 = new int[9] { 54, 55, 56, 63, 64, 65, 72, 73, 74 };
    private int[] chunk8 = new int[9] { 57, 58, 59, 66, 67, 68, 75, 76, 77 };
    private int[] chunk9 = new int[9] { 60, 61, 62, 69, 70, 71, 78, 79, 80 };
    private List<int[]> chunks = new List<int[]>();
    private int[] complete = new int[81] { 0,0,0,0,0,0,0,0,0,
                                             0,0,0,0,0,0,0,0,0,
                                              0,0,0,0,0,0,0,0,0,
                                               0,0,0,0,0,0,0,0,0,
                                                0,0,0,0,0,0,0,0,0,
                                                 0,0,0,0,0,0,0,0,0,
                                                  0,0,0,0,0,0,0,0,0,
                                                   0,0,0,0,0,0,0,0,0,
                                                    0,0,0,0,0,0,0,0,0 };
    private int[] generate = new int[81] { 0,0,0,0,0,0,0,0,0,
                                             0,0,0,0,0,0,0,0,0,
                                              0,0,0,0,0,0,0,0,0,
                                               0,0,0,0,0,0,0,0,0,
                                                0,0,0,0,0,0,0,0,0,
                                                 0,0,0,0,0,0,0,0,0,
                                                  0,0,0,0,0,0,0,0,0,
                                                   0,0,0,0,0,0,0,0,0,
                                                    0,0,0,0,0,0,0,0,0 };
    private int totalCorrect = 0;
    private int empty = 0;

    private static SudokuGenerator instance;
    public static SudokuGenerator Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        int difficulty = PlayerPrefs.GetInt("Difficulty");
        // Essentail for elimination od empty areas in sudoku grid
        AddChunks();
        // Generates new sudoku
        GenerateSudoku();
        fillComplete();

    }

    // Generates nww sudoku
    public void GenerateSudoku()
    {
        // This values can't be lower than 1
        Mathf.Clamp(clueWeight1, 1, int.MaxValue);
        Mathf.Clamp(clueWeight1, 1, int.MaxValue);
        Mathf.Clamp(mutations, 1, int.MaxValue);
        print("Max Value: " + int.MaxValue);
        // Some random numbers to eliminate often grid repetitions 
        int orderValue = Random.Range(1, 9);
        int rnd = Random.Range(1, 5);
        int rnd2 = Random.Range(1, 5);
        int reverse = Random.Range(1, 100);

        // we are creating sudoku grid
        if (rnd == 1 && rnd2 == 1)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (orderValue == 1)
                        field[i, j] = ((i * 3) + (i / 3) + j) % (3 * 3) + 1;
                    else if (orderValue == 2)
                        field[i, j] = ((i * 3) + (i / 3) + (8 - j)) % (3 * 3) + 1;
                    else if (orderValue == 3)
                        field[i, j] = (((8 - i) * 3) + ((8 - i) / 3) + j) % (3 * 3) + 1;
                    else if (orderValue == 2)
                        field[i, j] = (((8 - i)) * 3 + ((8 - i) / 3) + (8 - j)) % (3 * 3) + 1;
                    else if (orderValue == 6)
                        field[i, j] = ((i * 3) + ((8 - i) / 3) + (8 - j)) % (3 * 3) + 1;
                    else if (orderValue == 5)
                        field[i, j] = (((8 - i) * 3) + (i / 3) + (8 - j)) % (3 * 3) + 1;
                    else if (orderValue == 7)
                        field[i, j] = (((8 - i) * 3) + (i / 3) + j) % (3 * 3) + 1;
                    else if (orderValue == 8)
                        field[i, j] = ((i * 3) + ((8 - i) / 3) + j) % (3 * 3) + 1;
                }
            }
        }
        else if (rnd == 2 && rnd2 == 2)
        {
            for (int i = 8; i >= 0; i--)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (orderValue == 7)
                        field[i, j] = ((i * 3) + (i / 3) + j) % (3 * 3) + 1;
                    else if (orderValue == 5)
                        field[i, j] = ((i * 3) + (i / 3) + (8 - j)) % (3 * 3) + 1;
                    else if (orderValue == 3)
                        field[i, j] = (((8 - i) * 3) + ((8 - i) / 3) + j) % (3 * 3) + 1;
                    else if (orderValue == 4)
                        field[i, j] = (((8 - i)) * 3 + ((8 - i) / 3) + (8 - j)) % (3 * 3) + 1;
                    else if (orderValue == 2)
                        field[i, j] = ((i * 3) + ((8 - i) / 3) + (8 - j)) % (3 * 3) + 1;
                    else if (orderValue == 8)
                        field[i, j] = (((8 - i) * 3) + (i / 3) + (8 - j)) % (3 * 3) + 1;
                    else if (orderValue == 1)
                        field[i, j] = (((8 - i) * 3) + (i / 3) + j) % (3 * 3) + 1;
                    else if (orderValue == 6)
                        field[i, j] = ((i * 3) + ((8 - i) / 3) + j) % (3 * 3) + 1;
                }
            }
        }
        else if (rnd == 3 && rnd2 == 3)
        {
            for (int i = 8; i >= 0; i--)
            {
                for (int j = 8; j >= 0; j--)
                {
                    if (orderValue == 1)
                        field[i, j] = ((i * 3) + (i / 3) + j) % (3 * 3) + 1;
                    else if (orderValue == 2)
                        field[i, j] = ((i * 3) + (i / 3) + (8 - j)) % (3 * 3) + 1;
                    else if (orderValue == 3)
                        field[i, j] = (((8 - i) * 3) + ((8 - i) / 3) + j) % (3 * 3) + 1;
                    else if (orderValue == 4)
                        field[i, j] = (((8 - i)) * 3 + ((8 - i) / 3) + (8 - j)) % (3 * 3) + 1;
                    else if (orderValue == 5)
                        field[i, j] = ((i * 3) + ((8 - i) / 3) + (8 - j)) % (3 * 3) + 1;
                    else if (orderValue == 6)
                        field[i, j] = (((8 - i) * 3) + (i / 3) + (8 - j)) % (3 * 3) + 1;
                    else if (orderValue == 7)
                        field[i, j] = (((8 - i) * 3) + (i / 3) + j) % (3 * 3) + 1;
                    else if (orderValue == 8)
                        field[i, j] = ((i * 3) + ((8 - i) / 3) + j) % (3 * 3) + 1;
                }
            }
        }
        else
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 8; j >= 0; j--)
                {
                    if (orderValue == 8)
                        field[i, j] = ((i * 3) + (i / 3) + j) % (3 * 3) + 1;
                    else if (orderValue == 7)
                        field[i, j] = ((i * 3) + (i / 3) + (8 - j)) % (3 * 3) + 1;
                    else if (orderValue == 6)
                        field[i, j] = (((8 - i) * 3) + ((8 - i) / 3) + j) % (3 * 3) + 1;
                    else if (orderValue == 1)
                        field[i, j] = (((8 - i)) * 3 + ((8 - i) / 3) + (8 - j)) % (3 * 3) + 1;
                    else if (orderValue == 2)
                        field[i, j] = ((i * 3) + ((8 - i) / 3) + (8 - j)) % (3 * 3) + 1;
                    else if (orderValue == 3)
                        field[i, j] = (((8 - i) * 3) + (i / 3) + (8 - j)) % (3 * 3) + 1;
                    else if (orderValue == 4)
                        field[i, j] = (((8 - i) * 3) + (i / 3) + j) % (3 * 3) + 1;
                    else if (orderValue == 5)
                        field[i, j] = ((i * 3) + ((8 - i) / 3) + j) % (3 * 3) + 1;
                }
            }
        }

        // if we use mutations, go further...
        if (useMutations)
        {
            // mutation are basically rows and columns switching, e.g. we switch the position of first and third row or forth and fifth column.
            // of course, we can't switch first and sixth row beacase that combinations break sudoku.
            while (currentMutation < mutations)
            {
                rndNum = (((Random.Range(1, 3) * Random.Range(1, 3) * Random.Range(1, 3)) + (Random.Range(1, 3) * Random.Range(1, 3) * Random.Range(1, 3)) + (Random.Range(1, 3) * Random.Range(1, 3) * Random.Range(1, 3))) / 3) - 1;
                if (rndNum >= 0 && rndNum <= 2)
                {
                    _rndNum = Random.Range(0, 2);
                    while (_rndNum == rndNum)
                    {
                        _rndNum = Random.Range(0, 2);
                    }
                }
                else if (rndNum >= 3 && rndNum <= 5)
                {
                    _rndNum = Random.Range(3, 5);
                    while (_rndNum == rndNum)
                    {
                        _rndNum = Random.Range(3, 5);
                    }
                }
                else if (rndNum >= 6 && rndNum <= 8)
                {
                    _rndNum = Random.Range(6, 8);
                    while (_rndNum == rndNum)
                    {
                        _rndNum = Random.Range(6, 8);
                    }
                }

                for (int j = 0; j < 9; j++)
                {
                    hfield[0, j] = field[rndNum, j];
                    _hfield[0, j] = field[_rndNum, j];
                    field[rndNum, j] = _hfield[0, j];
                    field[_rndNum, j] = hfield[0, j];
                }

                for (int j = 0; j < 9; j++)
                {
                    vfield[j, 0] = field[j, rndNum];
                    _vfield[j, 0] = field[j, _rndNum];
                    field[j, rndNum] = _vfield[j, 0];
                    field[j, _rndNum] = vfield[j, 0];
                }

                currentMutation++;
            }
        }

        // should we reverse sudoku grid? chances are 50:50...
        if (reverse > 50)
            ReverseArray(field);

        currentMutation = 0;

        // finally, let's see the result.
        InstantiateTiles();
        //_sudokuParent.GetComponent<Renderer>().enabled = false;
    }

    // Instantiates sudoku grid and places generates sudoku numbers in apropriate place.
    void InstantiateTiles()
    {
        GameObject temp;
        string archive = "";

        // we need this parent object to easily destory the grid when we want new sudoku to be generated,
        //_sudokuParent = Instantiate(sudokuParent, transform.position, Quaternion.identity) as GameObject;
        _sudokuParent = Instantiate(sudokuParent, new Vector3(0, 200, 0), Quaternion.identity) as GameObject;
        // we need to instantiate 81 cube
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                
                temp = Instantiate(numberPrefab, new Vector3(i * 1 * numberPrefab.transform.localScale.x * spacing, 200, j * 1 * numberPrefab.transform.localScale.z * spacing), Quaternion.identity) as GameObject;
                temp.name = "tile_" + i.ToString() + "_" + j.ToString();
                temp.transform.eulerAngles = new Vector3(180, 0, 0);
                // every cube has some number that we assign here as text...
                temp.GetComponent<Letter>().letter.text = field[i,j].ToString();
                //...but also here as number. it can be any number from 1 to 9.
                temp.GetComponent<Letter>().value = field[i, j];
                // of course, you can't see every number. for now, we hide them all...
                temp.GetComponent<Letter>().visible = false;
                
                // ...and every one of them can later recive any value from 1 to 9, because modify is true...
                temp.GetComponent<Letter>().modify = true;
                
                 temp.GetComponent<Renderer>().materials[0].color = Color.black;
                temp.GetComponent<Renderer>().enabled = false;
                archive += field[i, j].ToString();
                tiles.Add(temp);
                temp.transform.parent = _sudokuParent.transform;
            }
        }

        // this part enables tracking of created sudoku grid.
        if (generates.Contains(archive))
            repeats++;
        else
            generates.Add(archive);

        // now it's time to show some clues to the user and discover some numbers
        if (useMaxClues)
        {
            ClueGeometryMax(maxCluesInRegion);
        }
        else
        {
            ClueGeometry();
        }
            
    }

    // Creates clue geometry for the grid. This functions is called when you set useMaxClues to false.
    void ClueGeometry()
    {
        int position;
        for (int i = 0; i < tiles.Count; i += 3)
        {
            for (int m = 0; m < clueWeight1; m++)
            {
                position = Random.Range(i, (i + clueWeight2));
                if (position > 80)
                {
                    continue;
                }
                // when we make clue, object that hold that value can't be modified and is visible from beginning
                tiles[position].GetComponent<Letter>().visible = true;
                tiles[position].GetComponent<Letter>().input = tiles[position].GetComponent<Letter>().value;
                tiles[position].GetComponent<Letter>().modify = false;
            }
        }

        if (checkEmptyGroups)
        {
            CheckEmpyGroups(true);
        }

        // split grid into nine areas.
        InstantiateBorders();
    }

    // Creates clue geometry for the grid. This functions is called when you set useMaxClues to true.
    void ClueGeometryMax(int max)
    {
        max = 1;
        List<GameObject> visibles = new List<GameObject>();
        int position;

        print("tiles count: " + tiles.Count);
        for (int i = 0; i < tiles.Count; i += 3)
        {
            for (int m = 0; m < clueWeight1; m++)
            {
                position = Random.Range(i, (i + clueWeight2));
                if (position > 80)
                {
                    continue;
                }
                tiles[position].GetComponent<Letter>().visible = true;
                tiles[position].GetComponent<Letter>().input = tiles[position].GetComponent<Letter>().value;
                tiles[position].GetComponent<Letter>().modify = false;

            }
        }
        foreach (int[] chunk in chunks)
        {
            for (int c = 0; c < 9; c++)
            {
                if (tiles[chunk[c]].GetComponent<Letter>().visible)
                {
                    visibles.Add(tiles[chunk[c]]);
                }
            }
            while (visibles.Count > max)
            {
                int pos = Random.Range(0, visibles.Count+1);
                if (pos < visibles.Count)
                {
                    visibles[pos].GetComponent<Letter>().input = 0;
                    visibles[pos].GetComponent<Letter>().visible = false;
                    visibles[pos].GetComponent<Letter>().modify = true;
                    visibles.RemoveAt(pos);
                   //TODO
                }
            }
            visibles.Clear();
        }

        if (checkEmptyGroups)
        {
            CheckEmpyGroups(true);
        }

        InstantiateBorders();
    }

    // Check for empty groups following rule that the largest total number of empty groups
    // (rows, columns, and squares) in a sudoku is can be nine.
    void CheckEmpyGroups(bool doubleCheck)
    {
        List<GameObject> objects = new List<GameObject>();
        foreach (int[] chunk in chunks)
        {
            if (tiles[chunk[0]].GetComponent<Letter>().input == 0 && tiles[chunk[1]].GetComponent<Letter>().input == 0 && tiles[chunk[2]].GetComponent<Letter>().input == 0)
            {
                objects.Add(tiles[chunk[0]]);
                empty++;
            }
            if (tiles[chunk[3]].GetComponent<Letter>().input == 0 && tiles[chunk[4]].GetComponent<Letter>().input == 0 && tiles[chunk[5]].GetComponent<Letter>().input == 0)
            {
                objects.Add(tiles[chunk[3]]);
                empty++;
            }
            if (tiles[chunk[6]].GetComponent<Letter>().input == 0 && tiles[chunk[7]].GetComponent<Letter>().input == 0 && tiles[chunk[8]].GetComponent<Letter>().input == 0)
            {
                objects.Add(tiles[chunk[6]]);
                empty++;
            }

            if (tiles[chunk[0]].GetComponent<Letter>().input == 0 && tiles[chunk[3]].GetComponent<Letter>().input == 0 && tiles[chunk[6]].GetComponent<Letter>().input == 0)
            {
                objects.Add(tiles[chunk[6]]);
                empty++;
            }
            if (tiles[chunk[1]].GetComponent<Letter>().input == 0 && tiles[chunk[4]].GetComponent<Letter>().input == 0 && tiles[chunk[7]].GetComponent<Letter>().input == 0)
            {
                objects.Add(tiles[chunk[7]]);
                empty++;
            }
            if (tiles[chunk[2]].GetComponent<Letter>().input == 0 && tiles[chunk[5]].GetComponent<Letter>().input == 0 && tiles[chunk[8]].GetComponent<Letter>().input == 0)
            {
                objects.Add(tiles[chunk[8]]);
                empty++;
            }
        }
        if (empty > 9)
        {
            for (int i = 0; i < empty - 9; i++)
            {
                if (i < objects.Count)
                {
                    objects[i].GetComponent<Letter>().visible = true;
                    objects[i].GetComponent<Letter>().input = objects[i].GetComponent<Letter>().value;
                    objects[i].GetComponent<Letter>().modify = false;
                }
            }
        }
    }

    // Instantiates borders that splits grid to nine groups.
    void InstantiateBorders()
    {
        GameObject temp;
        //Vertical Borders
        //temp = Instantiate(borderPrefab, new Vector3(((numberPrefab.transform.localScale.x * spacing) * 0) - ((1 * numberPrefab.transform.localScale.x * spacing) / 2), 10, ((numberPrefab.transform.localScale.z * spacing) * 4)), Quaternion.identity) as GameObject;
        //temp.transform.localScale = new Vector3(0.5f, 1, 9 * numberPrefab.transform.localScale.z * spacing);
        //temp.transform.parent = _sudokuParent.transform;
        temp = Instantiate(borderPrefab, new Vector3(((numberPrefab.transform.localScale.x * spacing) * 3) - ((1 * numberPrefab.transform.localScale.x * spacing) / 2), 10, ((numberPrefab.transform.localScale.z * spacing) * 4)), Quaternion.identity) as GameObject;
        temp.transform.localScale = new Vector3(0.5f, 1, 9 * numberPrefab.transform.localScale.z * spacing);
        temp.transform.parent = _sudokuParent.transform;
        temp = Instantiate(borderPrefab, new Vector3(((numberPrefab.transform.localScale.x * spacing) * 6) - ((1 * numberPrefab.transform.localScale.x * spacing) / 2), 10, ((numberPrefab.transform.localScale.z * spacing) * 4)), Quaternion.identity) as GameObject;
        temp.transform.localScale = new Vector3(0.5f, 1, 9 * numberPrefab.transform.localScale.z * spacing);
        temp.transform.parent = _sudokuParent.transform;
        //temp = Instantiate(borderPrefab, new Vector3(((numberPrefab.transform.localScale.x * spacing) * 9) - ((1 * numberPrefab.transform.localScale.x * spacing) / 2), 10, ((numberPrefab.transform.localScale.z * spacing) * 4)), Quaternion.identity) as GameObject;
        //temp.transform.localScale = new Vector3(0.5f, 1, 9 * numberPrefab.transform.localScale.z * spacing);
        //temp.transform.parent = _sudokuParent.transform;
        //Horizontal Borders
        //temp = Instantiate(borderPrefab, new Vector3(((numberPrefab.transform.localScale.x * spacing) * 4), 10, ((numberPrefab.transform.localScale.z * spacing) * 0) - ((1 * numberPrefab.transform.localScale.z * spacing) / 2)), Quaternion.identity) as GameObject;
        //temp.transform.localScale = new Vector3(9 * numberPrefab.transform.localScale.x * spacing, 1, 0.5f);
        ///temp.transform.parent = _sudokuParent.transform;
        temp = Instantiate(borderPrefab, new Vector3(((numberPrefab.transform.localScale.x * spacing) * 4), 10, ((numberPrefab.transform.localScale.z * spacing) * 3) - ((1 * numberPrefab.transform.localScale.z * spacing) / 2)), Quaternion.identity) as GameObject;
        temp.transform.localScale = new Vector3(9 * numberPrefab.transform.localScale.x * spacing, 1, 0.5f);
        temp.transform.parent = _sudokuParent.transform;
        temp = Instantiate(borderPrefab, new Vector3(((numberPrefab.transform.localScale.x * spacing) * 4), 10, ((numberPrefab.transform.localScale.z * spacing) * 6) - ((1 * numberPrefab.transform.localScale.z * spacing) / 2)), Quaternion.identity) as GameObject;
        temp.transform.localScale = new Vector3(9 * numberPrefab.transform.localScale.x * spacing, 1, 0.5f);
        temp.transform.parent = _sudokuParent.transform;
       // temp = Instantiate(borderPrefab, new Vector3(((numberPrefab.transform.localScale.x * spacing) * 4), 10, ((numberPrefab.transform.localScale.z * spacing) * 9) - ((1 * numberPrefab.transform.localScale.z * spacing) / 2)), Quaternion.identity) as GameObject;
        //temp.transform.localScale = new Vector3(9 * numberPrefab.transform.localScale.x * spacing, 1, 0.5f);
       // temp.transform.parent = _sudokuParent.transform;
    }

    // Check if sudoku has invalid entries.
    void CheckSudoku()
    {
        if (limitCheck)
        {
            if (_checkLimit > checkLimit)
            {
                return;
            }
        }

        _checkLimit++;

        check = true;
        foreach (GameObject tile in tiles)
        {
            if (tile.GetComponent<Letter>().visible)
            {
                if (tile.GetComponent<Letter>().input != tile.GetComponent<Letter>().value)
                {
                    
                        tile.GetComponent<Renderer>().materials[0].color = Color.red;
                    
                    
                }
            }
        }
        Invoke("ResetColors", 5);
    }

    // Shows all numbers.
    void ShowAll()
    {
        foreach (GameObject tile in tiles)
        {
           tile.GetComponent<Letter>().visible = true;
           tile.GetComponent<Letter>().modify = false;

        }
    }

    void fillComplete()
    {
        print(this.gameObject.name);
        GameObject gametile = this.gameObject.transform.parent.GetChild(1).gameObject;
        GameObject gamenum = this.gameObject.transform.parent.GetChild(9).gameObject;
        int k = 0;
        foreach (GameObject tile in tiles)
        {
            if(tile.GetComponent<Letter>().input > 0)
            {
                gametile.transform.GetChild(k).GetComponent<MeshRenderer>().material.mainTexture = gamenum.transform.GetChild(tile.GetComponent<Letter>().input - 1).GetComponent<SpriteRenderer>().material.mainTexture;
                generate.SetValue(tile.GetComponent<Letter>().input, k);
                totalCorrect++;
            }
            else
            {
                gametile.transform.GetChild(k).GetComponent<MeshRenderer>().material.mainTexture = gamenum.transform.GetChild(9).GetComponent<SpriteRenderer>().material.mainTexture;
				generate.SetValue(0, k);
            }
			gametile.transform.GetChild (k).GetComponent<MeshRenderer>().material.color = Color.white;
            complete.SetValue(tile.GetComponent<Letter>().value, k);
            k++;
        }
    }

    // Resets cube color to white after the checking is complited.
    void ResetColors()
    {
        foreach (GameObject tile in tiles)
        {
            tile.GetComponent<Renderer>().materials[0].color = Color.black;

        }
        check = false;
    }
   
    // Revereses genereted sudoku grid. Used for creation of more randomized sudoku.
    private static void ReverseArray(int[,] _array) 
    { 
        for (int rowIndex = 0; rowIndex <= (_array.GetUpperBound(0)); rowIndex++) { 
            for (int colIndex = 0; colIndex <= (_array.GetUpperBound(1) / 2); colIndex++) { 
                int tempHolder = _array[rowIndex, colIndex]; 
                _array[rowIndex, colIndex] = _array[rowIndex, _array.GetUpperBound(1) - colIndex]; 
                _array[rowIndex, _array.GetUpperBound(1) - colIndex] = tempHolder; 
            } 
        } 
    }
    
    // Update contains only input manipulation
    void Update()
    {

        GameObject clock = this.gameObject.transform.parent.GetChild(13).gameObject.transform.GetChild(0).gameObject;
        if (totalCorrect != 81)
        {
           
            string currentTime = clock.GetComponent<TextMesh>().text;
            cycle++;
            if (cycle == 60)
            {
                cycle = 0;
                seconds++;
                if (seconds == 60)
                {
                    seconds = 0;
                    minutes++;
                }
                if (minutes < 10 && seconds < 10)
                {
                    clock.GetComponent<TextMesh>().text = "0" + minutes.ToString() + ":" + "0" + seconds.ToString();
                }
                else
                {
                    if (minutes < 10 && seconds >= 10)
                    {
                        clock.GetComponent<TextMesh>().text = "0" + minutes.ToString() + ":" + seconds.ToString();
                    }
                    else
                    {
                        if (minutes >= 10 && seconds < 10)
                        {
                            clock.GetComponent<TextMesh>().text = minutes.ToString() + ":" + "0" + seconds.ToString();
                        }
                        else
                        {
                            clock.GetComponent<TextMesh>().text = minutes.ToString() + ":" + seconds.ToString();
                        }
                    }
                }

            }
        }
        if(totalCorrect == 81)
        {
            this.gameObject.transform.parent.GetChild(14).gameObject.SetActive(true);
            this.gameObject.transform.parent.GetChild(14).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text = clock.GetComponent<TextMesh>().text;

        }

       if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer)
        {
            // Left mouse button - increase value in non-clue field.
            if (Input.GetMouseButtonDown(0))
            {
                
                if (check)
                    ResetColors();
                
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {

                    GameObject g = hit.collider.gameObject;
                    string name = g.name;

                    int childposition = 0;
                    for(int i = 0; i < 81; i++)
                    {
                        if(g.transform.parent.GetChild(i).name == name)
                        {
                            childposition = i;
                            break;
                        }
                    }

                    if (hit.transform.parent.name.Contains("GameGrid"))
                    {

                        if(generate[childposition] == 0)
                        { 
                            switch (currentNumberValue)
                            {
                                case 0:
                                    g.GetComponent<MeshRenderer>().material.mainTexture = hit.collider.gameObject.transform.parent.transform.parent.GetChild(9).gameObject.transform.GetChild(9).gameObject.GetComponent<SpriteRenderer>().material.mainTexture;
                                    break;
                                case 1:
                                    g.GetComponent<MeshRenderer>().material.mainTexture = hit.collider.gameObject.transform.parent.transform.parent.GetChild(9).gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().material.mainTexture;
                                    break;
                                case 2:
                                    g.GetComponent<MeshRenderer>().material.mainTexture = hit.collider.gameObject.transform.parent.transform.parent.GetChild(9).gameObject.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().material.mainTexture;
                                    break;
                                case 3:
                                    g.GetComponent<MeshRenderer>().material.mainTexture = hit.collider.gameObject.transform.parent.transform.parent.GetChild(9).gameObject.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().material.mainTexture;
                                    break;
                                case 4:
                                    g.GetComponent<MeshRenderer>().material.mainTexture = hit.collider.gameObject.transform.parent.transform.parent.GetChild(9).gameObject.transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().material.mainTexture;
                                    break;
                                case 5:
                                    g.GetComponent<MeshRenderer>().material.mainTexture = hit.collider.gameObject.transform.parent.transform.parent.GetChild(9).gameObject.transform.GetChild(4).gameObject.GetComponent<SpriteRenderer>().material.mainTexture;
                                    break;
                                case 6:
                                    g.GetComponent<MeshRenderer>().material.mainTexture = hit.collider.gameObject.transform.parent.transform.parent.GetChild(9).gameObject.transform.GetChild(5).gameObject.GetComponent<SpriteRenderer>().material.mainTexture;
                                    break;
                                case 7:
                                    g.GetComponent<MeshRenderer>().material.mainTexture = hit.collider.gameObject.transform.parent.transform.parent.GetChild(9).gameObject.transform.GetChild(6).gameObject.GetComponent<SpriteRenderer>().material.mainTexture;
                                    break;
                                case 8:
                                    g.GetComponent<MeshRenderer>().material.mainTexture = hit.collider.gameObject.transform.parent.transform.parent.GetChild(9).gameObject.transform.GetChild(7).gameObject.GetComponent<SpriteRenderer>().material.mainTexture;
                                    break;
                                case 9:
                                    g.GetComponent<MeshRenderer>().material.mainTexture = hit.collider.gameObject.transform.parent.transform.parent.GetChild(9).gameObject.transform.GetChild(8).gameObject.GetComponent<SpriteRenderer>().material.mainTexture;
                                    break;
                                default:
                                    break;
                            }

                            if(currentNumberValue != complete[childposition])
                            {
                                getNumbersActive();
                                print(currentbuttonpressed);
                                if (currentbuttonpressed == "Clear_off")
                                {
                                    g.GetComponent<Renderer>().materials[0].color = Color.white;
                                }
                                else
                                {
                                    g.GetComponent<Renderer>().materials[0].color = Color.red;
                                }
                            }
                            else
                            {
                                g.GetComponent<MeshRenderer>().material.color = Color.white;
                                totalCorrect++;
                                generate.SetValue(currentNumberValue, childposition);
                            }

                        }
                    }

                    if (hit.transform.gameObject.name.Contains("tile_"))
                    {
                        // Allows user to double tap tile if it was incorrect
                        // To allow them to remove tile contents (Rather than
                        // having to press clear button.
                        if(hit.transform.gameObject.name != lasttilepressed)
                        {
                            lasttilepressed = hit.transform.gameObject.name;
                            getNumbersActive();
                            lastbuttonpressed = currentbuttonpressed;
                        }
                        else
                        {
                            getNumbersActive();
                           
                            if (lastbuttonpressed == currentbuttonpressed)
                            {
                                hit.transform.gameObject.GetComponent<Letter>().Erase();
                                hit.transform.gameObject.GetComponent<Renderer>().materials[0].color = Color.black;
                                lasttilepressed = "";
                                return;
                            }

                        }
                        GameObject onParent = this.transform.parent.gameObject;
                        bool gameoverActive;
                        gameoverActive = onParent.transform.GetChild(4).gameObject.activeSelf;
                        if (gameoverActive == true)
                        {
                            return;
                        }

                        if (currentNumberValue != 0)
                        {
                            
                            hit.transform.gameObject.GetComponent<Letter>().setValue(currentNumberValue);
                           
                            if (hit.transform.gameObject.GetComponent<Letter>().input == hit.transform.gameObject.GetComponent<Letter>().value)
                            {
                                hit.transform.gameObject.GetComponent<Letter>().letter.color = Color.white;
                                hit.transform.gameObject.GetComponent<Letter>().modify = false;
                            }
                            else
                            {
                                hit.transform.gameObject.GetComponent<Renderer>().materials[0].color = Color.red;
                            }
                        }
                        else
                        {
                            hit.transform.gameObject.GetComponent<Letter>().Erase();
                        }

                    }



                    if (name == "Yes")
                    {
                        totalCorrect = 0;
                        minutes = 0;
                        seconds = 0;
                        GameObject goParent = hit.collider.gameObject.transform.parent.gameObject;
                        goParent.SetActive(false);
                        Next();
                        System.Threading.Thread.Sleep(100);
                        fillComplete();

                    }

                    if (name == "No")
                    {
                        Application.LoadLevel("MainMenu");
                    }

                    if (name == "Gear")
                    {

                        GameObject onParent = this.transform.parent.gameObject;
                        bool gearActive = onParent.transform.GetChild(5).gameObject.activeSelf;
                        if (gearActive == true)
                        { 
                            onParent.transform.GetChild(5).gameObject.SetActive(false);
                        }
                        else
                        {
                            onParent.transform.GetChild(5).gameObject.SetActive(true);
                        }
                    }

                    if (name == "MusicOff")
                    {
                        GameObject settingsactive = hit.collider.gameObject.transform.parent.gameObject;
                        if (settingsactive.transform.GetChild(6).gameObject.activeSelf == true)
                        {
                            GameObject temp = GameObject.Find("Slider");
                            if (temp != null)
                            {
                                Slider volumeSlider;
                                volumeSlider = temp.GetComponent<Slider>();

                                if (volumeSlider != null)
                                {
                                    volumeSlider.normalizedValue = 0.5f;
                                }
                            }
                        }
                        GameObject goParent = hit.collider.gameObject.transform.parent.gameObject;
                        goParent.transform.GetChild(8).gameObject.SetActive(false);
                        System.Threading.Thread.Sleep(100);

                    }
                    if (name == "MusicOn")
                    {
                        GameObject settingsactive = hit.collider.gameObject.transform.parent.gameObject;
                        if (settingsactive.transform.GetChild(6).gameObject.activeSelf == true)
                        {
                            GameObject temp = GameObject.Find("Slider");
                            if (temp != null)
                            {
                                Slider volumeSlider;
                                volumeSlider = temp.GetComponent<Slider>();

                                if (volumeSlider != null)
                                {
                                    volumeSlider.normalizedValue = 0;
                                }
                            }
                        }
                        GameObject goParent = hit.collider.gameObject.transform.parent.gameObject;
                        goParent.transform.GetChild(8).gameObject.SetActive(true);
                        System.Threading.Thread.Sleep(100);
                    }
                    
                    
                    

                    switch (name)
                    {
                        case "1_off":
                            lastbuttonpressed = "1_off";
                            hitNumber = true;
                            break;
                        case "2_off":
                            lastbuttonpressed = "2_off";
                            hitNumber = true;
                            break;
                        case "3_off":
                            lastbuttonpressed = "3_off";
                            hitNumber = true;
                            break;
                        case "4_off":
                            lastbuttonpressed = "4_off";
                            hitNumber = true;
                            break;
                        case "5_off":
                            lastbuttonpressed = "5_off";
                            hitNumber = true;
                            break;
                        case "6_off":
                            lastbuttonpressed = "6_off";
                            hitNumber = true;
                            break;
                        case "7_off":
                            lastbuttonpressed = "7_off";
                            hitNumber = true;
                            break;
                        case "8_off":
                            lastbuttonpressed = "8_off";
                            hitNumber = true;
                            break;
                        case "9_off":
                            lastbuttonpressed = "9_off";
                            hitNumber = true;
                            break;
                        case "Clear_off":
                            lastbuttonpressed = "Clear_off";
                            hitNumber = true;
                            break;
                        default:
                            break;
                    }

                    if (hitNumber == true)
                    {
                        hitNumber = false;
                        setNumbersActive();
                        hit.collider.gameObject.SetActive(false);
                        checkNumbers();
                    }

                }
            }
            // Right mouse button - decrease value in non-clue field.
            /*
            if (Input.GetMouseButtonDown(1))
            {
                if (check)
                    ResetColors();

                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.gameObject.name.Contains("tile_"))
                        hit.transform.gameObject.GetComponent<Letter>().Decrease();
                }
            }
            // Scroll wheel - delete value from non-clue field
            if (Input.GetMouseButtonDown(2))
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.gameObject.name.Contains("tile_"))
                        hit.transform.gameObject.GetComponent<Letter>().Erase();
                }
            }
            */

        }

        // Current Android version support only left mouse button functionality.
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            for (int i = 0; i < Input.touchCount; ++i)
            {
                if (Input.GetTouch(i).phase.Equals(TouchPhase.Began))
                {
                    if (check)
                        ResetColors();

                    ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform.gameObject.name.Contains("tile_"))
                            hit.transform.gameObject.GetComponent<Letter>().Increase();

                        //////////////////////////////////////
                        for (int k = 0; k < 9; k++)
                        {
                            for (int j = 0; j < 9; j++)
                            {
                                GameObject temp;
                                temp = Instantiate(numberPrefab, new Vector3(k * 1 * numberPrefab.transform.localScale.x * spacing, 10, j * 1 * numberPrefab.transform.localScale.z * spacing), Quaternion.identity) as GameObject;


                                //...but also here as number. it can be any number from 1 to 9.
                                print(temp.GetComponent<Letter>().value);
                               
                            }
                        }




                    }
                }
            }
        }
    }

    void checkNumbers()
    {
        GameObject NumbersList = GameObject.Find("Numbers");
        
        for (int i = 0; i < 9; i++)
        {
            GameObject child_NumbersList = NumbersList.transform.GetChild(i).gameObject;
           
            if (child_NumbersList.activeSelf == false)
            {
                
                    currentNumberValue = i + 1;
                return;
            }
        }
        GameObject child_NumbersList2 = NumbersList.transform.GetChild(18).gameObject;

        if (child_NumbersList2.activeSelf == false)
        {

            currentNumberValue = 0;
            return;
        }
        return;
    }

    void setNumbersActive()
    {
        GameObject NumbersList = GameObject.Find("Numbers");

        for (int i = 0; i < 9; i++)
        {
            GameObject child_NumbersList = NumbersList.transform.GetChild(i).gameObject;
            child_NumbersList.SetActive(true);
            
        }
        GameObject child_NumbersList2 = NumbersList.transform.GetChild(18).gameObject;
        child_NumbersList2.SetActive(true);
        return;
    }

    void getNumbersActive()
    {
        GameObject NumbersList = GameObject.Find("Numbers");

        for (int i = 0; i < 9; i++)
        {
            GameObject child_NumbersList = NumbersList.transform.GetChild(i).gameObject;
            if (child_NumbersList.activeSelf == false)
            {
                currentbuttonpressed = child_NumbersList.name;
            }
        }
        GameObject child_NumbersList2 = NumbersList.transform.GetChild(18).gameObject;
        if (child_NumbersList2.activeSelf == false)
        {
            currentbuttonpressed = child_NumbersList2.name;
        }

        return;
    }

    void OnGUI()
    {


        // show all numbers
        if (GUI.Button(new Rect((Screen.width / 2) + 100, (Screen.height / 8) *7, 100, 30), "Quit"))
        {
            ShowAll();
            CheckSudoku();
            System.Threading.Thread.Sleep(400);
            GameObject onParent = this.transform.parent.gameObject;
            onParent.transform.GetChild(4).gameObject.SetActive(true);
          
        }

        //Check grid
     //   if (GUI.Button(new Rect((Screen.width / 2) + 100, (Screen.height / 20) * 19, 100, 30), "Check"))
     //   {
           
     //       CheckSudoku();
     //       System.Threading.Thread.Sleep(400);

     //   }

    }
 

    void AddChunks()
    {
        chunks.Add(chunk1);
        chunks.Add(chunk2); 
        chunks.Add(chunk3); 
        chunks.Add(chunk4); 
        chunks.Add(chunk5); 
        chunks.Add(chunk6); 
        chunks.Add(chunk7); 
        chunks.Add(chunk8); 
        chunks.Add(chunk9);
    }

    // create new sudoku and destroy old one
    public void Next()
    {
        tiles.Clear();
        Destroy(_sudokuParent);
        GenerateSudoku();
    }
}