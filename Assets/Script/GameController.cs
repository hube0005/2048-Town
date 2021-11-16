using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject _2, _4, _8, _16, _32, _64, _128, _256, _512, _1024, _2048;
    private int[,] grid = new int[4, 4];
    private Vector3[,] coordinatesGrid = new Vector3[4, 4];
    public GameObject mainCamera, secondCamera, endText;
    private bool isGameOver;

    void Start()
    {
        isGameOver = false;

        grid[0,0] = 2;
        grid[0,1] = 2;

        coordinatesGrid[0, 0] = new Vector3(-3.35f, 0.05f, 3.35f);
        coordinatesGrid[0, 1] = new Vector3(-1.15f, 0.05f, 3.35f);
        coordinatesGrid[0, 2] = new Vector3(1.15f, 0.05f, 3.35f);
        coordinatesGrid[0, 3] = new Vector3(3.35f, 0.05f, 3.35f);
        coordinatesGrid[1, 0] = new Vector3(-3.35f, 0.05f, 1.15f);
        coordinatesGrid[1, 1] = new Vector3(-1.15f, 0.05f, 1.15f);
        coordinatesGrid[1, 2] = new Vector3(1.15f, 0.05f, 1.15f);
        coordinatesGrid[1, 3] = new Vector3(3.35f, 0.05f, 1.15f);
        coordinatesGrid[2, 0] = new Vector3(-3.35f, 0.05f, -1.15f);
        coordinatesGrid[2, 1] = new Vector3(-1.15f, 0.05f, -1.15f);
        coordinatesGrid[2, 2] = new Vector3(1.15f, 0.05f, -1.15f);
        coordinatesGrid[2, 3] = new Vector3(3.35f, 0.05f, -1.15f);
        coordinatesGrid[3, 0] = new Vector3(-3.35f, 0.05f, -3.35f);
        coordinatesGrid[3, 1] = new Vector3(-1.15f, 0.05f, -3.35f);
        coordinatesGrid[3, 2] = new Vector3(1.15f, 0.05f, -3.35f);
        coordinatesGrid[3, 3] = new Vector3(3.35f, 0.05f, -3.35f);

        UpdateDisplay();
    }

    void Update()
    {
        char move = WaitForKeyPress(isGameOver);
        if (move != '_')
        {
            UpdateDisplay();

            isGameOver = IsFull() || is2048();

            if (isGameOver)
            {
                endText.SetActive(true);
            }
            else
            {
                Spawn(move);
                UpdateDisplay();
            }
        }
    }



    /** Attend et traite un mouvement 
     *  Le paramètre permet de bloquer les touches dé déplacement à la fin de la partie
     *  Retourne un char représentant le mouvement effectué
     *  **/
    private char WaitForKeyPress(bool isGameOver)
    {
        int k;
        char move = '_';

        if (!isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                move = 'u';

                // déplacement vers le plus haut possible
                for (int i = 0; i < 4; i++)
                {
                    // recherche de la première case vide de la colonne suivie d'une case pleine (parcours de haut en bas puis décalage à droite etc.)
                    k = 0;
                    while (k < 4 && grid[k, i] == 0)
                    { // tant que dernière case de la ligne pas passée ET case vide
                        k++; // recherche plus bas
                    }

                    if (k != 4)
                    { // si on a bien trouvé quelque chose

                        // on décale chaque case pleine vers le plus haut possible en commençant par la voisine de la case vide jusqu'à arriver la fin de la colonne
                        while (k < 4)
                        {
                            // pour toutes les lignes sauf celle du bord
                            for (int l = k; l > 0; l--)
                            {
                                // si la case au dessus est bien vide, déplacement vers le haut et case vidée
                                if (grid[l - 1, i] == 0)
                                {
                                    grid[l - 1, i] = grid[l, i];
                                    grid[l, i] = 0;
                                }
                            }
                            k++;
                        }

                    }
                }


                // fusion avec cases au dessus
                for (int i = 1; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (grid[i, j] == grid[i - 1, j])
                        {
                            grid[i - 1, j] *= 2;
                            grid[i, j] = 0;
                        }
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                move = 'd';

                // déplacement vers le plus bas possible
                for (int i = 0; i < 4; i++)
                {
                    // recherche de la première case vide de la colonne précédée d'une case pleine (parcours de bas en haut puis décalage à droite etc.)
                    k = 3;
                    while (k >= 0 && grid[k, i] == 0)
                    { // tant que première case de la ligne pas passée ET case vide
                        k--; // recherche plus haut
                    }


                    if (k != -1)
                    { // si on a bien trouvé quelque chose

                        // on décale chaque case pleine vers le plus bas possible en commençant par la voisine de la case vide jusqu'à arriver la fin de la colonne
                        while (k >= 0)
                        {
                            // pour toutes les lignes sauf celle du bord
                            for (int l = k; l < 3; l++)
                            {
                                // si la case en dessous est bien vide, déplacement vers le bas et case vidée
                                if (grid[l + 1, i] == 0)
                                {
                                    grid[l + 1, i] = grid[l, i];
                                    grid[l, i] = 0;
                                }
                            }
                            k--;
                        }

                    }
                }


                // fusion avec cases du dessous
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (grid[i, j] == grid[i + 1, j])
                        {
                            grid[i + 1, j] *= 2;
                            grid[i, j] = 0;
                        }
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                move = 'l';

                // déplacement vers le plus à gauche possible
                for (int i = 0; i < 4; i++)
                {
                    // recherche de la première case vide de la ligne suivie d'une case pleine (parcours de gauche à droite puis décalage en bas etc.)
                    k = 0;
                    while (k < 4 && grid[i, k] == 0)
                    { // tant que case vide et dernière case de la ligne pas passée
                        k++; // recherche à droite
                    }

                    if (k != 4)
                    { // si on a bien trouvé quelque chose

                        // on décale chaque case pleine vers le plus à gauche possible en commençant par la voisine de la case vide jusqu'à arriver la fin de la ligne
                        while (k < 4)
                        {
                            // pour toutes les colonnes sauf celle du bord
                            for (int l = k; l > 0; l--)
                            {
                                // si la case à gauche est bien vide, déplacement vers la gauche et case vidée
                                if (grid[i, l - 1] == 0)
                                {
                                    grid[i, l - 1] = grid[i, l];
                                    grid[i, l] = 0;
                                }
                            }
                            k++;
                        }

                    }
                }


                // fusion avec cases à gauche
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 1; j < 4; j++)
                    {
                        if (grid[i, j] == grid[i, j - 1])
                        {
                            grid[i, j - 1] *= 2;
                            grid[i, j] = 0;
                        }
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                move = 'r';

                // déplacement vers le plus à droite possible
                for (int i = 0; i < 4; i++)
                {
                    // recherche de la première case vide de la ligne suivie d'une case pleine (parcours de droite à gauche puis décalage en bas etc.)
                    k = 3;
                    while (k >= 0 && grid[i, k] == 0)
                    { // tant que case vide et premiere case de la ligne pas passée
                        k--;
                    }

                    if (k != -1)
                    { // si on a bien trouvé quelque chose

                        // on décale chaque case pleine vers le plus à droite possible en commençant par la voisine de la case vide jusqu'à arriver la fin de la ligne
                        while (k >= 0)
                        {
                            // pour toutes les colonnes sauf celle du bord
                            for (int l = k; l < 3; l++)
                            {
                                // si la case à droite est bien vide, déplacement vers la droite et case vidée
                                if (grid[i, l + 1] == 0)
                                {
                                    grid[i, l + 1] = grid[i, l];
                                    grid[i, l] = 0;
                                }
                            }
                            k--;
                        }

                    }
                }


                // fusion avec cases à droite
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (grid[i, j] == grid[i, j + 1])
                        {
                            grid[i, j + 1] *= 2;
                            grid[i, j] = 0;
                        }
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (mainCamera.activeSelf)
            {
                mainCamera.SetActive(false);
                secondCamera.SetActive(true);
            } else {
                mainCamera.SetActive(true);
                secondCamera.SetActive(false);
            }
        } else if (Input.GetKeyDown(KeyCode.R)) {
            NewGame();
        } else if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
        
        return move;
    }

    /** Permet de lancer une nouvelle partie **/
    private void NewGame()
    {
        grid = new int[4, 4];
        grid[0, 0] = 2;
        grid[0, 1] = 2;
        UpdateDisplay();

        isGameOver = false;
        endText.SetActive(false);
    }


    /** Vérifie si la grille est pleine
     *  Retourne true si oui, sinon false
     *  **/
    private bool IsFull()
    {
        bool isFull = true;

        // parcourt toute la grille pour savoir s'il y a au moins une case vide
        for(int i = 0; i < 4 && isFull; i++)
        {
            for(int j = 0; j < 4 && isFull; j++)
            {
                if (grid[i, j] == 0)
                    isFull = false;
            }
        }

        return isFull;
    }


    /**Vérifie la présence d'un 2048 **/
    private bool is2048()
    {
        bool res = false;

        for(int i = 0; i < 4; i++) {
            for(int j= 0; j < 4; j++) {
                if (grid[i, j] == 2048)
                    res = true;
            }
        }

        return res;
    }


    /** Ajoute une nouvelle valeur dans une case vide
     *  Le paramètre permet de connaitre le mouvement effectué et donc l'endroit où doit apparaitre la nouvelle valeur
     *  90% de chance qu'un 2 apparaisse et 10% pour le 4
     *  Pas de test pour savoir si la grille est pleine puisque déjà fait avant
     **/
    void Spawn(char move)
    {
        int pos, newValue;
        List<int> cells;

        // la nouvelle valeur qui sera ajoutée
        if(UnityEngine.Random.Range(0, 10) != 9) {
            newValue = 2;
        } else {
            newValue = 4;
        }

        switch (move) {
            case 'u':
                cells = IsLineFull(3); // vérifie si la ligne la plus en bas est vide

                if (cells.Count != 0) { // ajoute la valeur parmis les cases vides de la ligne la plus en bas
                    pos = cells[UnityEngine.Random.Range(0, cells.Count)];
                    grid[3, pos] = newValue;
                }

                break;
            case 'd':
                cells = IsLineFull(0); // vérifie si la ligne la plus en haut est vide

                if (cells.Count != 0) { // ajoute la valeur parmis les cases vides de la ligne la plus en haut
                    pos = cells[UnityEngine.Random.Range(0, cells.Count)];
                    grid[0, pos] = newValue;
                }

                break;
            case 'l':
                cells = IsColumnFull(3); // vérifie si la colonne la plus à droite est vide

                if (cells.Count != 0) { // ajoute la valeur parmis les cases vides de la ligne la plus à droite
                    pos = cells[UnityEngine.Random.Range(0, cells.Count)];
                    grid[pos, 3] = newValue;
                }

                break;
            case 'r':
                cells = IsColumnFull(0); // vérifie si la colonne la plus à gauche est vide

                if (cells.Count != 0) { // ajoute la valeur parmis les cases vides de la ligne la plus à gauche
                    pos = cells[UnityEngine.Random.Range(0, cells.Count)];
                    grid[pos, 0] = newValue;
                }

                break;
            case '_':
                // lancer exception
                break;
        }
    }

    /** Vérifie si la ligne l est pleine et retourne les indices des cases vides **/ 
    private List<int> IsLineFull(int l)
    {
        List<int> emptyCells = new List<int>();

        for(int i = 0; i < 4; i++) {
            if (grid[l, i] == 0)
                emptyCells.Add(i);
        }

        return emptyCells;
    }

    /** Vérifie si la colonne c est pleine et retourne les indices des cases vides **/
    private List<int> IsColumnFull(int c)
    {
        List<int> emptyCells = new List<int>();

        for (int i = 0; i < 4; i++)
        {
            if (grid[i, c] == 0)
                emptyCells.Add(i);
        }

        return emptyCells;
    }



    /** Mise à jour de l'affichage de la grille
     *  Supprime tous les objets de la grille et parcourt toute la grille pour les réajouter
     **/ 
    void UpdateDisplay()
    {
        GameObject[] buildings;

        buildings = GameObject.FindGameObjectsWithTag("Building");
        foreach (GameObject building in buildings)
        {
            Destroy(building);
        }


        for (int i = 0; i<4; i++)
        {
            for(int j = 0; j<4; j++)
            {
                if(grid[i,j] != 0)
                {
                    switch (grid[i, j])
                    {
                        case 2:
                            Instantiate(_2, coordinatesGrid[i, j], _2.gameObject.transform.rotation);
                            break;
                        case 4:
                            Instantiate(_4, coordinatesGrid[i, j], _4.gameObject.transform.rotation);
                            break;
                        case 8:
                            Instantiate(_8, coordinatesGrid[i, j], _8.gameObject.transform.rotation);
                            break;
                        case 16:
                            Instantiate(_16, coordinatesGrid[i, j], _16.gameObject.transform.rotation);
                            break;
                        case 32:
                            Instantiate(_32, coordinatesGrid[i, j], _32.gameObject.transform.rotation);
                            break;
                        case 64:
                            Instantiate(_64, coordinatesGrid[i, j], _64.gameObject.transform.rotation);
                            break;
                        case 128:
                            Instantiate(_128, coordinatesGrid[i, j], _128.gameObject.transform.rotation);
                            break;
                        case 256:
                            Instantiate(_256, coordinatesGrid[i, j], _256.gameObject.transform.rotation);
                            break;
                        case 512:
                            Instantiate(_512, coordinatesGrid[i, j], _512.gameObject.transform.rotation);
                            break;
                        case 1024:
                            Instantiate(_1024, coordinatesGrid[i, j], _1024.gameObject.transform.rotation);
                            break;
                        case 2048:
                            Instantiate(_2048, coordinatesGrid[i, j], _2048.gameObject.transform.rotation);
                            break;
                    }
                }
            }
        }
    }
}
