using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public static Map ins; //Instanssi

    public GameObject testi;

    public Vector2Int size;
    public int numberOfLand;
    public int numberOfForests;
    public int[,] mapData;
    public Sprite[] landTextures;
    public GameObject[] resources;
    public GameObject[] buildings;
    bool isGenerating = false;

    //KORJATTAVIA JUTTUA*** ?

    private void Awake()
    {
        {
            if (ins == null)
            {
                ins = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        } //Singleton...(?)

        mapData = new int[size.x, size.y];

        StartCoroutine(BeginNodeCreation());
    }

    List<Vector2Int> PossibleCoords(int type) //Hakee kartasta kaikki koordinaatit missä on annetun tyypin juttu, esim kaikki pisteet missä on merta.
    {
        List<Vector2Int> points = new List<Vector2Int>();
        for (int iy = 0; iy < size.y; iy++)
        {
            for (int ix = 0; ix < size.x; ix++)
            {
                if (mapData[ix, iy] == type)
                {
                    points.Add(new Vector2Int(ix, iy));
                }
            }
        }
        return points;
    }

    IEnumerator BeginNodeCreation ()
    {
        Vector2Int startingPoint = new Vector2Int(size.x / 2, size.y / 2);
        mapData[startingPoint.x, startingPoint.y] = (int)LandTypes.grass;

        List<int> allowedTypes = new List<int>();
        List<Vector2Int> edgePoints = new List<Vector2Int>();

        //Generoidaan maa
        isGenerating = true;
        allowedTypes.Add((int)LandTypes.sea);
        edgePoints.Add(startingPoint);

        StartCoroutine(CreateNode(numberOfLand, LandTypes.grass, allowedTypes, edgePoints));
        while (isGenerating) //isGenerating menee falseksi kun CreateNode on valmis
        {
            yield return new WaitForEndOfFrame();
        }
        allowedTypes.Clear();
        edgePoints.Clear();

        //Hiekka tulee veden reunalle niin ei tarvi alustaa mitään...
        GenerateSand();

        //Generoidaan puut
        for (int i = 0; i < numberOfForests; i++)
        {
            isGenerating = true;
            List<Vector2Int> possibleTreePoints = PossibleCoords((int)LandTypes.grass); //Mahdolliset paikat mihin puita voi laittaa
            startingPoint = possibleTreePoints[Random.Range(0, possibleTreePoints.Count - 1)]; //Valitaan sieltä jokin
            allowedTypes.Add((int)LandTypes.grass);
            edgePoints.Add(startingPoint);

            StartCoroutine(CreateNode(Random.Range(100, 500), LandTypes.tree, allowedTypes, edgePoints)); //HARDKOODATTU metsän kooksi 30 - 200
            while (isGenerating) //isGenerating menee falseksi kun CreateNode on valmis
            {
                yield return new WaitForEndOfFrame();
            }
            allowedTypes.Clear();
            edgePoints.Clear();
        }

        CreateWorldSprite();
        InstantiateResources();
    }

    bool IsInsideMap(Vector2Int pos)
    {
        if (pos.x >= 0 && pos.x < size.x && pos.y >= 0 && pos.y < size.y)
        {
            return true;
        }
        return false;
    }

    void GenerateSand()
    {
        List<Vector2Int> allGrass = PossibleCoords((int)LandTypes.grass);
        foreach (Vector2Int point in allGrass)
        {
            for (int iy = -2; iy <= 2; iy++)
            {
                for (int ix = -2; ix <= 2; ix++)
                {
                    Vector2Int pointBeingChecked = point + new Vector2Int(ix, iy);
                    if (IsInsideMap(pointBeingChecked))
                    {
                        if (mapData[pointBeingChecked.x, pointBeingChecked.y] == (int)LandTypes.sea)
                        {
                            mapData[point.x, point.y] = (int)LandTypes.sand;
                            break;
                        }
                    }
                }
            }
        }
    }

    IEnumerator CreateNode(int amount, LandTypes type, List<int> allowedTypes, List<Vector2Int> edgePoints)
    {
        amount--;
        if (amount < 0) //Jos kaikki maapalat on laitettu, niin lopetetaan. < 0 Jotta viimeinen palata tulee laitettua, == 0 ei tulisi...
        {
            isGenerating = false;
            yield break;
        }
        if (edgePoints.Count == 0)
        {
            isGenerating = false;
            yield break;
        }
        Vector2Int newOrigin = edgePoints[Random.Range(0, edgePoints.Count - 1)]; //Uusi piste minkä viereen lisätään uusi pala...
        List<Vector2Int> possiblePoints = FindPossibleNewSlots(newOrigin, allowedTypes);
        if (possiblePoints.Count == 0)
        {
            isGenerating = false;
            yield break;
        }
        Vector2Int newPoint = possiblePoints[Random.Range(0, possiblePoints.Count - 1)];
        
        mapData[newPoint.x, newPoint.y] = (int)type;

        List<Vector2Int> testList = FindPossibleNewSlots(newPoint, allowedTypes);
        if (testList.Count != 0) //Katotaan onko uuden pisteen ympärillä sallittua maata, jos on lisätään se reunapisteisiin
        {
            edgePoints.Add(newPoint);
        }
        testList = FindPossibleNewSlots(newOrigin, allowedTypes);

        for (int i = -1; i <= 1; i += 2)
        {
            if (IsInsideMap(newPoint + new Vector2Int(i, 0))) //Katotaan onko mapin sisällä
            {
                testList = FindPossibleNewSlots(newPoint + new Vector2Int(i, 0), allowedTypes);
                if (testList.Count == 0) //Katotaan onko pisteen, minkä ympäriltä löydettiin uus piste, sallittu maa(?). Jos ei ole niin poistetaan se reunapisteistä.
                {
                    edgePoints.Remove(newPoint + new Vector2Int(i, 0));
                }
            }
        }

        for (int i = -1; i <= 1; i += 2)
        {
            if (IsInsideMap(newPoint + new Vector2Int(0, i))) //Katotaan onko mapin sisällä
            {
                testList = FindPossibleNewSlots(newPoint + new Vector2Int(0, i), allowedTypes);
                if (testList.Count == 0) //Katotaan onko pisteen, minkä ympäriltä löydettiin uus piste, vettä. Jos ei ole niin poistetaan se reunapisteistä.
                {
                    edgePoints.Remove(newPoint + new Vector2Int(0, i));
                }
            }
        }

        if (amount % 100 == 0)
        {
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(CreateNode(amount, type, allowedTypes, edgePoints));
    }

    List<Vector2Int> FindPossibleNewSlots(Vector2Int origin, List<int> allowedTypes) //AllowedTypes on siis lista sallituista maista. Esim jos generoidaan maata, niin sitä saa asettaa vain veden päälle joten Allowedypes[0] = LandTypes.sea eikä siinä ole muita alkioita. Puiden kanssa se olisi sitten maa eli LandTypes.grass jne...
    {
        List<Vector2Int> possiblePoints = new List<Vector2Int>();
        for (int i = -1; i <= 1; i += 2)
        {
            if (IsInsideMap(origin + new Vector2Int(i, 0))) //Katotaan onko mapin sisällä
            {
                if (allowedTypes.Contains(mapData[origin.x + i, origin.y]))
                {
                    possiblePoints.Add(new Vector2Int(origin.x + i, origin.y));
                }
            }
        }

        for (int i = -1; i <= 1; i += 2)
        {
            if (IsInsideMap(origin + new Vector2Int(0, i))) //Katotaan onko mapin sisällä
            {
                if (allowedTypes.Contains(mapData[origin.x, origin.y + i]))
                {
                    possiblePoints.Add(new Vector2Int(origin.x, origin.y + i));
                }
            }
        }
        return possiblePoints;
    }

    void CreateWorldSprite()
    {
        Texture2D worldTexture = new Texture2D(size.x * 32, size.y * 32, TextureFormat.ARGB32, false);

        for (int iy = 0; iy < size.y; iy++)
        {
            for (int ix = 0; ix < size.x; ix++)
            {
                Texture2D landTexture;
                if (mapData[ix, iy] == 0)
                {
                    landTexture = landTextures[0].texture;
                }
                else if (mapData[ix, iy] == (int)LandTypes.sand)
                {
                    landTexture = landTextures[(int)LandTypes.sand].texture;
                }
                else
                {
                    landTexture = landTextures[1].texture;
                }
                Graphics.CopyTexture(landTexture, 0, 0, 0, 0, 32, 32, worldTexture, 0, 0, ix * 32, iy * 32);
            }
        }
        worldTexture.filterMode = FilterMode.Point;
        Sprite worldSprite = Sprite.Create(worldTexture, new Rect(0, 0, worldTexture.width, worldTexture.height), new Vector2(0, 0), 32);
        gameObject.AddComponent<SpriteRenderer>().sprite = worldSprite;
    }

    void InstantiateResources()
    {
        for (int iy = 0; iy < size.y; iy++)
        {
            for (int ix = 0; ix < size.x; ix++)
            {
                if (mapData[ix, iy] == (int)LandTypes.tree)
                {
                    Instantiate(resources[0], new Vector3(ix + Random.Range(0, 0.3f), iy + Random.Range(0, 0.3f), 0), Quaternion.identity).transform.parent = transform; //Luodaan ja laitetaan samalla mapin lapseksi niin ei näytä niin täydeltä editorissa... Hox että esineiden koordinaatti on vasemmassa alareunassa.
                }
            }
        }
    }//Sotkeeko randomi paikka???

    float HeuresticEstimate(Vector2Int a, Vector2Int b)
    {
        Vector2 BtoA = a - b;
        return BtoA.magnitude;
    }

    List<Vector2Int> ReconstructPath(IDictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
    {
        List<Vector2Int> totalPath = new List<Vector2Int>();
        totalPath.Add(current);
        List<Vector2Int> keys = new List<Vector2Int>(cameFrom.Keys);
        while (keys.Contains(current))//alkupistettä ei koskaan tule cameFrommiinn....
            {
            current = cameFrom[current];
            totalPath.Add(current);
            }
        totalPath.Reverse();
        return totalPath;
    }

    public List<Vector2Int> AStarPathFinding(Vector2Int start, Vector2Int goal)
    {
        if (!GM.allowedLand.Contains((LandTypes)mapData[goal.x, goal.y]))
        {
            return new List<Vector2Int>() { new Vector2Int(-999, -999) };
        }
        if (start == goal)
        {
            return new List<Vector2Int>();
        }
        List<Vector2Int> closedSet = new List<Vector2Int>(); //Pisteet jotka on jo tutkittu?
        List<Vector2Int> openSet = new List<Vector2Int>(); //Pisteet mitkä pitää tutkia?
        openSet.Add(start);
        IDictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>(); //Tyhjä kartta aluksi. Lopuksi jollakin avaimella saadaan piste mistä kannattee mennä siihen pisteeseen. cameFrom[jokinPiste] = lyhinPiste tästä tuohon johonkin pisteeseen(?) EHKÄ?
        IDictionary<Vector2Int, float> gScore = new Dictionary<Vector2Int, float>(); //Hinta alkupisteetä tähän.
        gScore[start] = 0;
        IDictionary<Vector2Int, float> fScore = new Dictionary<Vector2Int, float>(); //Koko matkan hinta tänne?
        fScore[start] = HeuresticEstimate(start, goal);
        while (openSet.Count != 0)
        {
            Vector2Int current = SmallestFScoreFromOpenSet(openSet, fScore);
            if (current == goal)
            {
                return ReconstructPath(cameFrom, current);
            }
            openSet.Remove(current);
            closedSet.Add(current);
            List<Vector2Int> neighbors = FindNeighbors(current);
            foreach (Vector2Int neighbor in neighbors)
            {
                if (closedSet.Contains(neighbor))
                {
                    continue;
                }
                float tentativeGScore = gScore[current] + (current - neighbor).magnitude;
                if (!openSet.Contains(neighbor))
                {
                    openSet.Add(neighbor);
                }
                else if (tentativeGScore >= gScore[neighbor])
                {
                    continue;
                }
                
                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeGScore;
                fScore[neighbor] = tentativeGScore + HeuresticEstimate(neighbor, goal);
                if (neighbor == goal)
                {
                    break;
                }
            }
        }
        return new List<Vector2Int>(); //Jos ei löydy polkua niin tulee tyhjä lista...
    }

    public List<Vector2Int> AStarPathFinding(Vector2Int start, Vector2Int goal, List<Vector2Int> buildingCoordinates, Vector2Int buildingSize)
    {
        if (start == goal)
        {
            return new List<Vector2Int>();
        }
        if (!IsBuildingAccessible(goal, buildingSize))
        {
            return new List<Vector2Int>() { new Vector2Int(-999, -999) };
        }
        List<Vector2Int> closedSet = new List<Vector2Int>(); //Pisteet jotka on jo tutkittu?
        List<Vector2Int> openSet = new List<Vector2Int>(); //Pisteet mitkä pitää tutkia?
        openSet.Add(start);
        IDictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>(); //Tyhjä kartta aluksi. Lopuksi jollakin avaimella saadaan piste mistä kannattee mennä siihen pisteeseen. cameFrom[jokinPiste] = lyhinPiste tästä tuohon johonkin pisteeseen(?) EHKÄ?
        IDictionary<Vector2Int, float> gScore = new Dictionary<Vector2Int, float>(); //Hinta alkupisteetä tähän.
        gScore[start] = 0;
        IDictionary<Vector2Int, float> fScore = new Dictionary<Vector2Int, float>(); //Koko matkan hinta tänne?
        fScore[start] = HeuresticEstimate(start, goal);
        while (openSet.Count != 0)
        {
            Vector2Int current = SmallestFScoreFromOpenSet(openSet, fScore);
            if (current == goal)
            {
                return ReconstructPath(cameFrom, current);
            }
            openSet.Remove(current);
            closedSet.Add(current);
            List<Vector2Int> neighbors = FindNeighbors(current, buildingCoordinates);
            foreach (Vector2Int neighbor in neighbors)
            {
                if (closedSet.Contains(neighbor))
                {
                    continue;
                }
                float tentativeGScore = gScore[current] + (current - neighbor).magnitude;
                if (!openSet.Contains(neighbor))
                {
                    openSet.Add(neighbor);
                }
                else if (tentativeGScore >= gScore[neighbor])
                {
                    continue;
                }
                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeGScore;
                fScore[neighbor] = tentativeGScore + HeuresticEstimate(neighbor, goal);
                if (neighbor == goal)
                {
                    break;
                }
            }
        }
        return new List<Vector2Int>(); //Jos ei löydy polkua niin tulee tyhjä lista...
    }

    List<Vector2Int> FindNeighbors(Vector2Int point, List<Vector2Int> buildingCoordinates) //PITÄÄKÖ TEHÄ ALLOWED LAND TYPES JOSKUS??? JOO PITÄÄ JA TEHTY
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        for (int iy = -1; iy <= 1; iy++)
        {
            for (int ix = -1; ix <= 1; ix++)
            {
                Vector2Int pointBeingChecked = new Vector2Int(point.x + ix, point.y + iy);
                if (IsInsideMap(pointBeingChecked) && !(ix == 0 && iy == 0))
                {
                    if (GM.allowedLand.Contains((LandTypes)mapData[pointBeingChecked.x, pointBeingChecked.y]) || buildingCoordinates.Contains(pointBeingChecked))
                    {
                        neighbors.Add(pointBeingChecked);
                    }
                }
            }
        }
        return neighbors;
    }

    List<Vector2Int> FindNeighbors(Vector2Int point) //Versio missä ei tarvitse antaa Rakennuksen pisteitä
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        for (int iy = -1; iy <= 1; iy++)
        {
            for (int ix = -1; ix <= 1; ix++)
            {
                Vector2Int pointBeingChecked = new Vector2Int(point.x + ix, point.y + iy);
                if (IsInsideMap(pointBeingChecked) && !(ix == 0 && iy == 0))
                {
                    if (GM.allowedLand.Contains((LandTypes)mapData[pointBeingChecked.x, pointBeingChecked.y]))
                    {
                        neighbors.Add(pointBeingChecked);
                    }
                }
            }
        }
        return neighbors;
    }

    Vector2Int SmallestFScoreFromOpenSet(List<Vector2Int> openSet, IDictionary<Vector2Int, float> fScore)
    {
        float compare = 9999;
        Vector2Int smallestPoint = openSet[0];
        foreach (Vector2Int point in openSet)
        {
            if (fScore.ContainsKey(point) &&fScore[point] < compare)
            {
                smallestPoint = point;
                compare = fScore[point];
            }
        }
        return smallestPoint;
    }

    List<Vector2Int> PointsAroundBuilding(Vector2Int point, Vector2Int size)
    {
        List<Vector2Int> possiblePoints = new List<Vector2Int>();
        for (int iy = 0; iy <= 1; iy++) //Käydään läpi rakennuksen ala ja ylä reuna...
        {
            for (int ix = 0; ix < size.x; ix++)
            {
                Vector2Int pointBeingChecked = point + new Vector2Int(ix, (size.y - 1) * iy + (2 * iy - 1)); //(2 * iy - 1) tekee sen että kun iy = 0 niin katsotaan rakennuksen alareunaa, ja sitte iy= 1 niin yläreunaa. (Siis reunasta yks kauemmaksi(?))
                if (IsInsideMap(pointBeingChecked))
                {
                    if (GM.allowedLand.Contains((LandTypes)mapData[pointBeingChecked.x, pointBeingChecked.y]))
                    {
                        possiblePoints.Add(pointBeingChecked);
                    }
                }
                if (ix == 0)
                {
                    Vector2Int newPoint = pointBeingChecked + new Vector2Int(-1, 0);//vasen alareuna ja yläreuna
                    if (IsInsideMap(newPoint))
                    {
                        if (GM.allowedLand.Contains((LandTypes)mapData[newPoint.x, newPoint.y]))
                        {
                            possiblePoints.Add(newPoint);
                        }
                    }
                }
                if (ix == size.x - 1)
                {
                    Vector2Int newPoint = pointBeingChecked + new Vector2Int(1, 0);//oikea alareuna ja yläreuna
                    if (IsInsideMap(newPoint))
                    {
                        if (GM.allowedLand.Contains((LandTypes)mapData[newPoint.x, newPoint.y]))
                        {
                            possiblePoints.Add(newPoint);
                        }
                    }
                }
            }
        }
        for (int ix = 0; ix <= 1; ix++)//Käydään läpi rakennuksen vasen ja oikea reuna...
        {
            for (int iy = 0; iy < size.y; iy++)
            {
                Vector2Int pointBeingChecked = point + new Vector2Int((size.x - 1) * ix + (2 * ix - 1), iy);
                if (IsInsideMap(pointBeingChecked))
                {
                    if (GM.allowedLand.Contains((LandTypes)mapData[pointBeingChecked.x, pointBeingChecked.y]))
                    {
                        possiblePoints.Add(pointBeingChecked);
                    }
                }
            }
        }
        return possiblePoints;
    }

    List<Vector2Int> BuildingCoordinates(Vector2Int point, Vector2Int size)
    {
        List<Vector2Int> coordinates = new List<Vector2Int>();
        for (int ix = 0; ix < size.x; ix++)
        {
            for (int iy = 0; iy < size.y; iy++)
            {
                Vector2Int pointBeingChecked = point + new Vector2Int(ix, iy);
                coordinates.Add(pointBeingChecked);
            }
        }
        return coordinates;
    }

    public List<Vector2Int> CorrectPathToBuilding(Vector2Int start, Vector2Int goal, Vector2Int buildingPoint, Vector2Int buildingSize)
    {
        List<Vector2Int> path = AStarPathFinding(start, goal, BuildingCoordinates(buildingPoint, buildingSize), buildingSize);
        if (path.Count == 1 && path[0] == new Vector2Int(-999, -999))
        {
            return path;
        }
        List<Vector2Int> coordinatesAroundBuilding = PointsAroundBuilding(buildingPoint, buildingSize);
        for (int i = path.Count - 1; i > 0; i--)
        {
            if (!coordinatesAroundBuilding.Contains(path[i]))
            {
                path.RemoveAt(i);
            }
            else
            {
                return path;
            }
        }
        return new List<Vector2Int>(); //Jos ei toiminutkaan?
    }//Käytä tätä kun haet reittiä rakennukseen, etkä yhteen pisteeseen

    bool IsBuildingAccessible(Vector2Int point, Vector2Int size)
    {
        List<Vector2Int> pointsAroundBuilding = PointsAroundBuilding(point, size);
        if (pointsAroundBuilding.Count == 0)
        {
            return false;
        }
        return true;
    } //Pystyykö rakennuksen tykö edes menemään? Eli onko rakennuksen vieressä AllowedLand

    public bool CanBeBuilt(Vector2Int point, Vector2Int size) //Palauttaa true jos kyseisen rakennukset voi rakentaa tähän pisteeseen
    {
        for (int iy = 0; iy < size.y; iy++)
        {
            for (int ix = 0; ix < size.x; ix++)
            {
                Vector2Int testPoint = point + new Vector2Int(ix, iy);
                if (!(IsInsideMap(testPoint) && GM.allowedLand.Contains((LandTypes)mapData[testPoint.x, testPoint.y])))
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void AddBuildingToMap(Vector2Int point, Vector2Int size, GameObject building)
    {
        for (int iy = 0; iy < size.y; iy++)
        {
            for (int ix = 0; ix < size.x; ix++)
            {
                Vector2Int testPoint = point + new Vector2Int(ix, iy);
                mapData[testPoint.x, testPoint.y] = (int)LandTypes.building;
            }
        }
        Instantiate(buildings[0], new Vector3(point.x, point.y, 0), Quaternion.identity);
    }

    public Vector2Int FindClosestLand(Vector2Int start, LandTypes type, int maxNodesChecked, List<Vector2Int> alreadyChecked)//alreadyChecked esim. jos satutaan valitsemaan sellainen puu joka on juuri kuollut. niiin lisätään se piste jo katsottuun listaan niin saadaan uusi puuuu
    {
        Vector2Int testPos = start;
        for (int sideLength = 1; true; sideLength++)
        {
            for (int ii = 0; ii <= 1; ii++)
            {
                for (int i = 0; i < sideLength; i++)
                {
                    if (sideLength % 2 == 0)
                    {
                        if (ii == 0)
                        {
                            testPos += new Vector2Int(0, -1);
                        }
                        else
                        {
                            testPos += new Vector2Int(-1, 0);
                        }
                    }
                    else
                    {
                        if (ii == 0)
                        {
                            testPos += new Vector2Int(0, 1);
                        }
                        else
                        {
                            testPos += new Vector2Int(1, 0);
                        }
                    }
                    if (alreadyChecked.Contains(testPos))
                    {
                        continue;
                    }
                    if (mapData[testPos.x, testPos.y] == (int)type)
                    {
                        return testPos;
                    }
                    maxNodesChecked--;
                    if (maxNodesChecked == 0)
                    {
                        return new Vector2Int(-999, -999);
                    }
                }
            }
        }
    }
}