using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRenderer : MonoBehaviour
{
    [SerializeField] private GameObject cam;
    public Vector2 chunkSize;

    private Vector2 centerSpacement;

    private Vector2 actualCenterPosition = Vector2.zero;

    private MapGenerator mapGenerator;

    private void Awake()
    {
        mapGenerator = this.GetComponent<MapGenerator>();
        centerSpacement = mapGenerator.centerSpacement;
    }

    private void Start()
    {
        QueryChunkUpdate();
    }

    private Vector2 RoundToNearestChunk()
    {
        Vector2 _camPos = new Vector2((cam.transform.position.y * 2) - -cam.transform.position.x,
             (cam.transform.position.y * 2) + -cam.transform.position.x);
        Vector2 _distanceBetweenChunks = chunkSize;

        return new Vector2(Mathf.Round(_camPos.x / _distanceBetweenChunks.x) * _distanceBetweenChunks.x,
            Mathf.Round(_camPos.y / _distanceBetweenChunks.y) * _distanceBetweenChunks.y);
    }

    private void Update()
    {
        Vector2 _actualPos = RoundToNearestChunk();
        if (actualCenterPosition != _actualPos)
        {
            actualCenterPosition = _actualPos;
            QueryChunkUpdate();
        }
    }

    public void QueryChunkUpdate()
    {
        //Reset chunks
        mapGenerator.ResetChunks();

        //Center chunk
        mapGenerator.DrawChunk(actualCenterPosition);

        //Horizontal and Vertical chunks
        mapGenerator.DrawChunk(new Vector2(actualCenterPosition.x + chunkSize.x, actualCenterPosition.y));
        mapGenerator.DrawChunk(new Vector2(actualCenterPosition.x - chunkSize.x, actualCenterPosition.y));
        mapGenerator.DrawChunk(new Vector2(actualCenterPosition.x, actualCenterPosition.y + chunkSize.y));
        mapGenerator.DrawChunk(new Vector2(actualCenterPosition.x, actualCenterPosition.y - chunkSize.y));

        //Diagonal Chunks
        mapGenerator.DrawChunk(new Vector2(actualCenterPosition.x + chunkSize.x, actualCenterPosition.y + chunkSize.y));
        mapGenerator.DrawChunk(new Vector2(actualCenterPosition.x - chunkSize.x, actualCenterPosition.y - chunkSize.y));
        mapGenerator.DrawChunk(new Vector2(actualCenterPosition.x + chunkSize.x, actualCenterPosition.y - chunkSize.y));
        mapGenerator.DrawChunk(new Vector2(actualCenterPosition.x - chunkSize.x, actualCenterPosition.y + chunkSize.y));
    }
}
