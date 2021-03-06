using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlapyBird : MonoBehaviour
{
    [SerializeField] private GameObject visual;
    [SerializeField] private float m_jump_force;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float m_speed;
    [SerializeField] private bool m_isDeath = false;

    [SerializeField] private System.Action OnTouchWall;
    [SerializeField] private System.Action<Vector3> OnTouchWallV3;
    [SerializeField] private System.Action OnTouchSpike;

    [SerializeField] private LayerMask wallLayer = 0;
    [SerializeField] private LayerMask spikeLayer = 0;

    private Vector3 pos;

    private Animator animator;

    public void Init(ref System.Action OnTouchWall,ref System.Action OnTouchSpike, System.Action<Vector3> OnTouchWallV3)
    {
        this.OnTouchWall = OnTouchWall;
        this.OnTouchSpike = OnTouchSpike;
        this.OnTouchWallV3 = OnTouchWallV3;
    }
    public void MyReset()
    {
        transform.position = pos;
        transform.rotation = Quaternion.identity;
        _rb.velocity = transform.right * m_speed;
        _rb.angularVelocity = 0f;
        _rb.constraints = RigidbodyConstraints2D.None;
        m_isDeath = false;
        visual.transform.eulerAngles = new Vector3(0, 0, 0);
    }
    private void OnEnable()
    {
        _rb.velocity = transform.right * m_speed;
    }
    private void Start()
    {
        pos = transform.position;
        animator = GetComponent<Animator>();
        _rb.velocity = transform.right * m_speed;
    }
    private void Update()
    {
        if (m_isDeath)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            print("OnClick");
            _rb.velocity = new Vector3(_rb.velocity.x, 0f, 0f);
            _rb.AddForce( Vector3.up*m_jump_force);
            animator.SetTrigger("Jump");
        }
        Quaternion q = Quaternion.AngleAxis(_rb.velocity.y, transform.forward);
        Vector3 v = visual.transform.eulerAngles;
        visual.transform.eulerAngles = new Vector3(v.x, v.y, q.eulerAngles.z);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (m_isDeath)
            return ;
        print("OnTriggerEnter");
        Vector3 save = _rb.velocity;
        if ((wallLayer.value & (1 << other.gameObject.layer)) > 0)
        {
            if (_rb.velocity.x > 0)
                visual.transform.eulerAngles = new Vector3(0, 180, 0);
            else
                visual.transform.eulerAngles = new Vector3(0, 0, 0);
            _rb.velocity = new Vector3(-save.x, save.y, -save.z);
            OnTouchWall?.Invoke();
            OnTouchWallV3?.Invoke(transform.position);
        }
        if ((spikeLayer.value & (1 << other.gameObject.layer)) > 0)
        {
            print("spikeTouch");
            _rb.velocity = new Vector3(-save.x, save.y, -save.z);
            _rb.AddTorque(95,ForceMode2D.Impulse);
            //aminStart;
            OnTouchSpike?.Invoke();
            m_isDeath = true;
        }
    }
}
