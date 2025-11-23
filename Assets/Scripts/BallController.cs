using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    public float maxPower;
    public float changeAngleSpeed;
    public float lineLength;
    public Slider powerSlider;
    public TextMeshProUGUI puttCountLabel;
    public float minHoleTime;
    public Transform startTransform;
    public LevelManager levelManager;

    private LineRenderer line;
    private Rigidbody ball;
    private float angle;
    private float powerUpTime;
    private float power;
    private int putts;
    private float holeTime;
    private Vector3 lastPosition;

    void Awake()
    {
        ball = GetComponent<Rigidbody>();
        if (ball != null)
            ball.maxAngularVelocity = 1000;

        line = GetComponent<LineRenderer>();
        if (line == null)
        {
            line = gameObject.AddComponent<LineRenderer>();
        }

        if (startTransform != null)
        {
            var renderer = startTransform.GetComponent<MeshRenderer>();
            if (renderer != null)
                renderer.enabled = false;
        }
    }

    void Update()
    {
        if (ball == null) return;

        // Ball must be completely stopped to aim or shoot
        if (ball.angularVelocity.magnitude < 0.01f)
        {
            lastPosition = transform.position;

            if (Input.GetKey(KeyCode.A)) ChangeAngle(-1);
            if (Input.GetKey(KeyCode.D)) ChangeAngle(1);
            if (Input.GetKey(KeyCode.Space)) PowerUp();
            if (Input.GetKeyUp(KeyCode.Space)) Putt();

            if (holeTime == 0) UpdateLinePositions();
        }
        else
        {
            if (line != null)
                line.enabled = false;
        }
    }

    private void ChangeAngle(int direction)
    {
        angle += changeAngleSpeed * Time.deltaTime * direction;
        angle = Mathf.Repeat(angle, 360f);
    }

    private void UpdateLinePositions()
    {
        if (line == null) return;
        line.enabled = true;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position + Quaternion.Euler(0, angle, 0) * Vector3.forward * lineLength);
    }

    private void Putt()
    {
        lastPosition = transform.position;
        if (ball != null)
            ball.AddForce(Quaternion.Euler(0, angle, 0) * Vector3.forward * maxPower * power, ForceMode.Impulse);

        power = 0;
        powerUpTime = 0;
        if (powerSlider != null) powerSlider.value = 0;

        putts++;
        if (puttCountLabel != null)
        {
            puttCountLabel.ForceMeshUpdate(); // TMP-safe
            puttCountLabel.text = putts.ToString();
        }
    }

    private void PowerUp()
    {
        powerUpTime += Time.deltaTime;
        power = Mathf.PingPong(powerUpTime, 1);
        if (powerSlider != null) powerSlider.value = power;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Hole")) CountHoleTime();
    }

    private void CountHoleTime()
    {
        holeTime += Time.deltaTime;
        if (holeTime >= minHoleTime)
        {
            if (levelManager != null)
                levelManager.NextPlayer(putts);
            holeTime = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hole")) holeTime = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Out Of Bounds"))
        {
            transform.position = lastPosition;
            if (ball != null) ball.angularVelocity = Vector3.zero;
        }
    }

    public void SetupBall(Color color)
    {
        if (startTransform != null)
            transform.position = startTransform.position;

        angle = startTransform != null ? startTransform.rotation.eulerAngles.y : 0f;

        if (ball != null)
            ball.angularVelocity = Vector3.zero;

        var renderer = GetComponent<MeshRenderer>();
        if (renderer != null)
            renderer.material.color = color;

        if (line != null)
            line.material.color = color;

        if (line != null) line.enabled = true;

        putts = 0;
        if (puttCountLabel != null)
        {
            puttCountLabel.ForceMeshUpdate();
            puttCountLabel.text = "0";
        }
    }
}
