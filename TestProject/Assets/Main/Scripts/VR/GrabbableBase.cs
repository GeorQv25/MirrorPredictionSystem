using Game.Player;
using Mirror;
using UnityEngine;
using UnityEngine.Events;


public abstract class GrabbableBase : NetworkBehaviour
{
    public bool ActiveR { private set; get; }
    public bool ActiveL { private set; get; }

    public UnityEvent onTake;
    public UnityEvent onRelease;
    
    [SerializeField] protected Transform offsetR;
    [SerializeField] protected Transform offsetL;
    [SerializeField] private bool switchOwner;

    protected Vector3 _speed;
    private Vector3 _oldPos;
    
    protected Transform _posHandR;
    protected Transform _posHandL;

    protected Quaternion _startRot;
    protected Quaternion _startRotHand;
    
    protected Transform _localPosR;
    protected Transform _localPosL;

    private NetworkRigidbodyUnreliable _netRb;
    protected Rigidbody _rb;
    
    private void Start()
    {
        TryGetComponent(out _rb);
        TryGetComponent(out _netRb);
        _oldPos = transform.position;
        _localPosR = new GameObject("PointR").transform;
        _localPosR.parent = transform;
        _localPosL = new GameObject("PointL").transform;
        _localPosL.parent = transform;
        OnStart();
    }

    protected virtual void OnStart(){}

    public abstract Transform SetHand(TypeHand hand, bool active, Transform posHand);

    protected Transform InitHands(TypeHand hand, bool active, Transform posHand)
    {
        Transform offset;
        
        if (hand == TypeHand.Right)
        {
            ActiveR = active;
            _posHandR = posHand;
            _startRot = transform.rotation;
            _startRotHand = _posHandR.rotation;
            offset = offsetR;
        }
        else
        {
            ActiveL = active;
            _posHandL = posHand;
            _startRot = transform.rotation;
            _startRotHand = _posHandL.rotation;
            offset = offsetL;
        }

        if (ActiveR | ActiveL)
        {
            _rb.isKinematic = true;
            CmdPickup(NetworkServer.localConnection);
            onTake?.Invoke();
        }
        else
        {
            transform.parent = null;
            _rb.isKinematic = false;
            _rb.velocity = _speed * 3;
            CmdPickup();
            onRelease?.Invoke();
        } 
     
        return offset;
    }
    
    
    private void FixedUpdate()
    {
        if (!ActiveR & !ActiveL)
            return;

        if (ActiveR & _posHandR)
        {
           TakeR();

            //Если держит только правая
            if (!ActiveL)
            {
                TakeOnlyR();
            }
        }

        if (ActiveL & _posHandL)
        {
            TakeL();

            //Если держит только левая
            if (!ActiveR)
            {
               TakeOnlyL();
            }
        }

        //Если держат обе
        if (ActiveR & ActiveL)
        {
            TakeTwoHands();
        }
        
        GetSpeed();
    }

    /// <summary>
    /// Держит правая (независимо от левой)
    /// </summary>
    protected abstract void TakeR();
    
    /// <summary>
    /// Держит только правая рука
    /// </summary>
    protected abstract void TakeOnlyR();
    
    /// <summary>
    /// Держит левая рука (независимо от правой)
    /// </summary>
    protected abstract void TakeL();
    
    /// <summary>
    /// Держит только левая рука
    /// </summary>
    protected abstract void TakeOnlyL();
    
    /// <summary>
    /// Держат обе руки
    /// </summary>
    protected abstract void TakeTwoHands();
    
    
    protected void RotateWrist(Transform posHand, Transform localPos, Transform wristRotator)
    {
        Vector3 point = posHand.position + posHand.forward;
        localPos.position = point;
        point = localPos.localPosition;
        point.Normalize();
        float rotZ = 0;
        float x = point.x;
        float y = point.y;
        rotZ = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        wristRotator.localEulerAngles = new Vector3(0, 0, rotZ);
    }
    
    protected void GetSpeed()
    {
        _speed = (transform.position - _oldPos) / Time.fixedDeltaTime;
        _oldPos = transform.position;
    }
    
    [Command(requiresAuthority = false)]
    private void CmdPickup(NetworkConnectionToClient sender = null)
    {
        if(!switchOwner)
            return;
        if (sender == null)
        {
            netIdentity.RemoveClientAuthority();
            _netRb.syncDirection = SyncDirection.ServerToClient;
            RpcChangeSyncDirection(SyncDirection.ServerToClient);
        }
        else if (sender != netIdentity.connectionToClient)
        {
            netIdentity.RemoveClientAuthority();
            netIdentity.AssignClientAuthority(sender);
            _netRb.syncDirection = SyncDirection.ClientToServer;
            RpcChangeSyncDirection(SyncDirection.ClientToServer);
        }
    }

    [ClientRpc]
    private void RpcChangeSyncDirection(SyncDirection newSyncDirection)
    {
        _netRb.syncDirection = newSyncDirection;
    }
}
