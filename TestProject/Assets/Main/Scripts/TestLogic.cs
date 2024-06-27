using System.Collections;
using UnityEngine;


public class TestLogic : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 1.0f;
    [SerializeField] private float _timeToPause = 1.0f;
    [SerializeField] RectTransform _rectTransform;
    [SerializeField] private float _bottomZ;


    private IEnumerator Start()
    {

        yield return new WaitForSeconds(2);
        yield return MoveText();
    }

    private IEnumerator MoveText()
    {
        Vector3 currentPos = transform.position;
        Vector3 bottomPos = new Vector3(currentPos.x, currentPos.y, _bottomZ);
        Vector3 centerPos = new Vector3(currentPos.x, currentPos.y, 0);

        while (Vector3.SqrMagnitude((transform.position) - centerPos) > 0.1f)
        {
            _rectTransform.position = Vector3.Lerp(transform.position, centerPos, Time.deltaTime * _movementSpeed);
            yield return null;
        }

        yield return new WaitForSeconds(2);

        while (Vector3.SqrMagnitude((transform.position) - bottomPos) > 0.1f)
        {
            _rectTransform.position = Vector3.Lerp(transform.position, bottomPos, Time.deltaTime * _movementSpeed);
            yield return null;
        }
    }
}