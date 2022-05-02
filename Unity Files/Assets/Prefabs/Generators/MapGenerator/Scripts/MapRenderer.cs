using System.Collections.Generic;
using UnityEngine;

public class MapRenderer : MonoBehaviour
{
    [SerializeField] private GameObject cam;
    public Vector2 chunkSize;

    private Vector2 actualCenterPosition = Vector2.zero;

    private MapGenerator mapGenerator;
    private List<Vector2> renderedChunks = new List<Vector2>();

    private void Awake()
    {
        mapGenerator = this.GetComponent<MapGenerator>();
    }

    private void Start()
    {
        //The first rendering query
        QueryChunkUpdate();
    }

    private Vector2 RoundToNearestChunk()
    {
        //Convert the camera position to isometric position
        Vector2 _camPos = new Vector2((cam.transform.position.y * 2) - -cam.transform.position.x,
             (cam.transform.position.y * 2) + -cam.transform.position.x);
        Vector2 _distanceBetweenChunks = chunkSize;

        //Get the nearest chunk based on the player position
        return new Vector2(Mathf.Round(_camPos.x / _distanceBetweenChunks.x) * _distanceBetweenChunks.x,
            Mathf.Round(_camPos.y / _distanceBetweenChunks.y) * _distanceBetweenChunks.y);
    }

    private void Update()
    {
        //Verify if the player has entered in a new chunk and query for update
        Vector2 _actualPos = RoundToNearestChunk();
        if (actualCenterPosition != _actualPos)
        {
            actualCenterPosition = _actualPos;
            QueryChunkUpdate();
        }
    }

    public void QueryChunkUpdate()
    {
        List<Vector2> _chunksToRender = new List<Vector2>();
        List<Vector2> _renderedChunks = new List<Vector2>();
        List<Vector2> _chunksToRemove = new List<Vector2>();
        List<Vector2> _chunksToSave = new List<Vector2>();

        //Add center and adjacent chunks to the render list
        _chunksToRender.Add(actualCenterPosition);
        _chunksToRender.Add(new Vector2(actualCenterPosition.x + chunkSize.x, actualCenterPosition.y));
        _chunksToRender.Add(new Vector2(actualCenterPosition.x - chunkSize.x, actualCenterPosition.y));
        _chunksToRender.Add(new Vector2(actualCenterPosition.x, actualCenterPosition.y + chunkSize.y));
        _chunksToRender.Add(new Vector2(actualCenterPosition.x, actualCenterPosition.y - chunkSize.y));
        _chunksToRender.Add(new Vector2(actualCenterPosition.x + chunkSize.x, actualCenterPosition.y + chunkSize.y));
        _chunksToRender.Add(new Vector2(actualCenterPosition.x - chunkSize.x, actualCenterPosition.y - chunkSize.y));
        _chunksToRender.Add(new Vector2(actualCenterPosition.x + chunkSize.x, actualCenterPosition.y - chunkSize.y));
        _chunksToRender.Add(new Vector2(actualCenterPosition.x - chunkSize.x, actualCenterPosition.y + chunkSize.y));

        //Delete unnecessary chunks
        for (var i = 0; i < renderedChunks.Count; i++)
        {
            if (!_chunksToRender.Contains(renderedChunks[i]))
            {
                _chunksToRemove.Add(renderedChunks[i]);
            }
        }
        mapGenerator.RemoveChunks(_chunksToRemove);

        //Search for repeated chunks and draw the rest
        foreach (Vector2 _chunk in _chunksToRender)
        {
            if (!renderedChunks.Contains(_chunk))
            {
                _renderedChunks.Add(_chunk);
            }
            _chunksToSave.Add(_chunk);
        }
        mapGenerator.DrawChunk(_renderedChunks);

        //Save actual rendered chunks
        renderedChunks = _chunksToSave;
    }

    public void ReloadChunks()
    {
        //Just delete all chunks and redraw them
        mapGenerator.RemoveChunks(renderedChunks);
        renderedChunks.Clear();
        QueryChunkUpdate();
    }
}
