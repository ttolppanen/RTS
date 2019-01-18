using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public static Map ins; //Instanssi

    public Vector2Int size;
    public int numberOfLand;
    public int[,] mapData;
    public Sprite[] landTextures;
    public GameObject[] resources;
    List<Vector2Int> edgePoints = new List<Vector2Int>();

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

        Vector2Int startingPoint = new Vector2Int(Random.Range(0, size.x), Random.Range(0, size.y));
        mapData[startingPoint.x, startingPoint.y] = Random.Range(1, (int)LandTypes.lastNumber);
        edgePoints.Add(startingPoint);
        StartCoroutine(CreateMap());
    }

    IEnumerator CreateMap()
    {
        numberOfLand--;
        if (numberOfLand < 0) //Jos kaikki maapalat on laitettu, niin lopetetaan. < 0 Jotta viimeinen palata tulee laitettua, == 0 ei tulisi...
        {
            CreateWorldSprite();
            InstantiateResources();
            yield break;
        }

        Vector2Int newOrigin = edgePoints[Random.Range(0, edgePoints.Count - 1)]; //Uusi piste minkä viereen lisätään uusi pala...
        List<Vector2Int> possiblePoints = FindPossibleNewSlots(newOrigin);
        Vector2Int newPoint = possiblePoints[Random.Range(0, possiblePoints.Count - 1)];
        if (Random.Range(0f, 1f) >= 0.8f)
        {
            mapData[newPoint.x, newPoint.y] = mapData[newOrigin.x, newOrigin.y];
        }
        else
        {
            mapData[newPoint.x, newPoint.y] = Random.Range(1, (int)LandTypes.lastNumber - 1);
        }

        List<Vector2Int> testList = FindPossibleNewSlots(newPoint);
        if (testList.Count != 0) //Katotaan onko uuden pisteen ympärillä vettä, jos on niin lisätään se reunapisteisiin...
        {
            edgePoints.Add(newPoint);
        }
        testList = FindPossibleNewSlots(newOrigin);

        for (int i = -1; i <= 1; i += 2)
        {
            if (newPoint.x + i >= 0 && newPoint.x + i < size.x) //Katotaan onko mapin sisällä
            {
                testList = FindPossibleNewSlots(newPoint + new Vector2Int(i, 0));
                if (testList.Count == 0) //Katotaan onko pisteen, minkä ympäriltä löydettiin uus piste, vettä. Jos ei ole niin poistetaan se reunapisteistä.
                {
                    edgePoints.Remove(newPoint + new Vector2Int(i, 0));
                }
            }
        }

        for (int i = -1; i <= 1; i += 2)
        {
            if (newPoint.y + i >= 0 && newPoint.y + i < size.y) //Katotaan onko mapin sisällä
            {
                testList = FindPossibleNewSlots(newPoint + new Vector2Int(0, i));
                if (testList.Count == 0) //Katotaan onko pisteen, minkä ympäriltä löydettiin uus piste, vettä. Jos ei ole niin poistetaan se reunapisteistä.
                {
                    edgePoints.Remove(newPoint + new Vector2Int(0, i));
                }
            }
        }

        if (numberOfLand % 100 == 0)
        {
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(CreateMap());
    }

    List<Vector2Int> FindPossibleNewSlots(Vector2Int origin)
    {
        List<Vector2Int> possiblePoints = new List<Vector2Int>();
        for (int i = -1; i <= 1; i += 2)
        {
            if (origin.x + i >= 0 && origin.x + i < size.x) //Katotaan onko mapin sisällä
            {
                if (mapData[origin.x + i, origin.y] == 0)
                {
                    possiblePoints.Add(new Vector2Int(origin.x + i, origin.y));
                }
            }
        }

        for (int i = -1; i <= 1; i += 2)
        {
            if (origin.y + i >= 0 && origin.y + i < size.y) //Katotaan onko mapin sisällä
            {
                if (mapData[origin.x, origin.y + i] == 0)
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
                    Instantiate(resources[0], new Vector3(ix, iy, 0), Quaternion.identity);
                }
            }
        }
    }
}
public enum LandTypes {sea, grass, tree, sand, lastNumber};