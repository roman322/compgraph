using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineTank : MonoBehaviour
{

    //швидкість переміщення танку
    float MoveSpeed = 1.5f;

    //швидкість повороту танка
    float RotateSpeed = 20;

    //теперішня швидкість танку
    float CurrentSpeed = 0;

    //передача танку
    int SpeedNum = 0;

    //получаємо компонент руху танку
    Rigidbody TankEngine;

    public GameObject Tower;
    public GameObject Bullet;
    public GameObject StartGun;

    void OnGUI()
    {
        GUI.Label(new Rect(new Vector2(10, 10), new Vector2(100, 20)), "T" + SpeedNum + "Speed" + (CurrentSpeed * MoveSpeed * Time.deltaTime));
    }
    
    void OnCollisionEnter(Collision collision)
    {
        SpeedNum = 0;
    }

    void Fire()
    {
        if (Input.GetButtonUp("Fire4"))
        {
            Vector3 SpawnPoint = StartGun.transform.position;
            Quaternion SpawnRoot = StartGun.transform.rotation;
            GameObject BulletForFire = Instantiate(Bullet, SpawnPoint, SpawnRoot) as GameObject;
            Rigidbody Run = BulletForFire.GetComponent<Rigidbody>();
            Run.AddForce(BulletForFire.transform.forward * 30, ForceMode.Impulse);
            Destroy(BulletForFire, 1);
        }
    }

    void Move()
    {
        //розраховуємо куди буде рухатись танк
        Vector3 Move = transform.forward * CurrentSpeed * MoveSpeed * Time.deltaTime;
        //получаємо позицію танку
        Vector3 Pose = TankEngine.position + Move;
        //пробуємо рухати танк в вказаному напрямку
        TankEngine.MovePosition(Pose);
    }

    void Rotates()
    {
        //розраховуємо поворот
        float R = Input.GetAxis("Horizontal") * RotateSpeed * Time.deltaTime;

        //створюємо новий кут повороту танку
        Quaternion RotateAngle = Quaternion.Euler(0, R, 0);

        //получаємо теперішній кут повороту танка
        Quaternion CurrentAngle = TankEngine.rotation * RotateAngle;

        //повертаємо танк
        TankEngine.MoveRotation(CurrentAngle);

    }

    void RotateTower()
    {
        float AngleRotate = Time.deltaTime * RotateSpeed * Input.GetAxis("MoveTower");
        Tower.transform.Rotate(0, 0, AngleRotate);
    }

    void FixedUpdate()
    {
        Fire();

        Transmission();

        //рухаємо танк
        Move();

        //повертаємо танк
        Rotates();

        RotateTower();
    }

    // Start is called before the first frame update
    void Start()
    {
        //получаємо компонент руху танку
        TankEngine = GetComponent<Rigidbody>();


    }

    // Update is called once per frame
    void Update()
    {
        //запамятовуємо що нажав користувач
        float Axis = Input.GetAxis("Vertical");

        //збільшення і зменшення швидкості
        if (Input.GetButtonUp("Vertical"))
        {
            if (Axis > 0) UpSpeed();
            if (Axis < 0) DownSpeed();
        }
    }

    void UpSpeed()
    {
        //якщо швидкість не вище 6 то будемо пришвидшувати танк
        if ((SpeedNum + 1) < 6) SpeedNum++;
    }

    void DownSpeed()
    {
        if ((SpeedNum - 1) > -2) SpeedNum--;
    }

    void Transmission()
    {
        switch (SpeedNum)
        {
            case -1: CurrentSpeed = -1; RotateSpeed = 25; break;//задній хід
            case 0: CurrentSpeed = 0; RotateSpeed = 20; break;//паркінг
            case 1: CurrentSpeed = 0.5f; RotateSpeed = 25; break;
            case 2: CurrentSpeed = 1f; RotateSpeed = 30; break;
            case 3: CurrentSpeed = 1.5f; RotateSpeed = 35; break;
            case 4: CurrentSpeed = 2f; RotateSpeed = 40; break;
            case 5: CurrentSpeed = 2.5f; RotateSpeed = 45; break;
        }
    }
}
