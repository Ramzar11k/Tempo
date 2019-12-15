using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage1Script : MonoBehaviour {

    public int stageProgression;
    GameObject player;
    GameObject sword;
    GameObject arrow;

    GameObject doors;
    GameObject energyBars;
    Color drained;
    AudioSource audios;

    public GameObject boss;
    public Transform bossLocation;
    public Transform respawnPoint;
    public Transform cameraLocation;
    public int classChosen;

    List<GameObject> crystals = new List<GameObject>();

	void Start ()
    {
        audios = GetComponent<AudioSource>();
        for (var i = 0; i < transform.GetChild(2).GetChild(0).childCount - 1; i++)
            crystals.Add(transform.GetChild(2).GetChild(0).GetChild(i).gameObject);
        drained = transform.GetChild(4).GetChild(0).GetComponent<Renderer>().material.color;
        energyBars = transform.GetChild(2).GetChild(0).GetChild(4).gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        doors = transform.GetChild(2).GetChild(1).gameObject;
        sword = transform.GetChild(1).GetChild(3).gameObject;
        arrow = transform.GetChild(1).GetChild(4).gameObject;
    }
	

	void Update ()
    {
        ChooseClass();
        Stage();
        Doors();
	}

    void Doors()
    {
        if (crystals[0].GetComponent<ActivationCrystalScript>().active == true)
        {
            OpenDoor(1);
            OpenDoor(2);
        }
        if (crystals[1].GetComponent<ActivationCrystalScript>().active == true && crystals[2].GetComponent<ActivationCrystalScript>().active == true && crystals[3].GetComponent<ActivationCrystalScript>().active == true)
            OpenDoor(0);
    }

    void Stage()
    {
        switch (stageProgression)
        {
            case 1:
                StartCoroutine(WaitAndProgress(2f));
                stageProgression++;
                break;
            case 2:
                transform.GetChild(1).GetChild(2).transform.Translate(Vector3.up * Time.deltaTime * 1f);
                break;
            case 3:
                break;
            case 4:
                StartCoroutine(WaitAndProgress(2f));
                DisableCollider(0);
                stageProgression++;
                break;
            case 5:
                for (var i = 0; i < transform.GetChild(1).GetChild(1).childCount; i++)
                    transform.GetChild(1).GetChild(1).GetChild(i).GetComponent<MeshRenderer>().enabled = false;
                transform.GetChild(1).GetChild(2).transform.Translate(Vector3.down * Time.deltaTime * 1f);
                break;
            case 6:
                break;
            case 7:
                for (var i = 0; i < transform.GetChild(3).childCount; i++)
                    transform.GetChild(3).GetChild(i).gameObject.SetActive(false);
                for (var i = 0; i < transform.GetChild(2).GetChild(3).childCount; i++)
                    transform.GetChild(2).GetChild(3).GetChild(i).GetComponent<MeshRenderer>().enabled = false;
                DisableCollider(1);
                transform.GetChild(0).GetChild(2).GetComponent<BoxCollider>().enabled = true;
                stageProgression++;
                break;
        }
    }
    
    IEnumerator WaitAndProgress(float time)
    {
        yield return new WaitForSeconds(time);
        stageProgression++;
        yield return null;
    }

    public IEnumerator DrainBar(float time)
    {
        yield return new WaitForSeconds(time);
        if (stageProgression == 8)
        {
            energyBars.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.color = drained;
            yield return new WaitForSeconds(time);
            energyBars.transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material.color = drained;
            yield return new WaitForSeconds(time);
            energyBars.transform.GetChild(0).GetChild(2).GetComponent<Renderer>().material.color = drained;
            yield return new WaitForSeconds(time);
            energyBars.transform.GetChild(0).GetChild(3).GetComponent<Renderer>().material.color = drained;
            yield return new WaitForSeconds(time);
            energyBars.transform.GetChild(0).GetChild(4).GetComponent<Renderer>().material.color = drained;
            yield return new WaitForSeconds(time);
            audios.pitch = 1.3f;
        }
        if (stageProgression == 9)
        {
            energyBars.transform.GetChild(1).GetChild(0).GetComponent<Renderer>().material.color = drained;
            yield return new WaitForSeconds(time);
            energyBars.transform.GetChild(1).GetChild(1).GetComponent<Renderer>().material.color = drained;
            yield return new WaitForSeconds(time);
            energyBars.transform.GetChild(1).GetChild(2).GetComponent<Renderer>().material.color = drained;
            yield return new WaitForSeconds(time);
            energyBars.transform.GetChild(1).GetChild(3).GetComponent<Renderer>().material.color = drained;
            yield return new WaitForSeconds(time);
            energyBars.transform.GetChild(1).GetChild(4).GetComponent<Renderer>().material.color = drained;
            yield return new WaitForSeconds(time);
            audios.pitch = 1.65f;
        }
        if (stageProgression == 10)
        {
            energyBars.transform.GetChild(2).GetChild(0).GetComponent<Renderer>().material.color = drained;
            yield return new WaitForSeconds(time);
            energyBars.transform.GetChild(2).GetChild(1).GetComponent<Renderer>().material.color = drained;
            yield return new WaitForSeconds(time);
            energyBars.transform.GetChild(2).GetChild(2).GetComponent<Renderer>().material.color = drained;
            yield return new WaitForSeconds(time);
            energyBars.transform.GetChild(2).GetChild(3).GetComponent<Renderer>().material.color = drained;
            yield return new WaitForSeconds(time);
            energyBars.transform.GetChild(2).GetChild(4).GetComponent<Renderer>().material.color = drained;
            yield return new WaitForSeconds(time);
            audios.pitch = 2f;
        }
        stageProgression++;
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            stageProgression++;
        }
    }

    void DisableCollider(int colliderNo)
    {
        transform.GetChild(0).GetChild(colliderNo).GetComponent<BoxCollider>().enabled = false;
    }
    void ChooseClass()
    {
        if (Input.GetKeyDown(KeyCode.Space) && stageProgression == 0)
        {
            if (Vector3.Distance(player.transform.position, sword.transform.position) <= 1f && Vector3.Distance(player.transform.position, arrow.transform.position) <= 1f)
            {
                var x = Random.Range(0, 2);
                if (x == 0)
                    player.GetComponent<SwordMaster>().enabled = true;
                if (x == 1)
                    player.GetComponent<Archer>().enabled = true;
                Destroy(sword);
                Destroy(arrow);
                stageProgression++;
            }
            else if (Vector3.Distance(player.transform.position, sword.transform.position) <= 1f && Vector3.Distance(player.transform.position, arrow.transform.position) > 1f)
            {
                player.GetComponent<SwordMaster>().enabled = true;
                Destroy(sword);
                Destroy(arrow);
                stageProgression++;
            }
            else if (Vector3.Distance(player.transform.position, sword.transform.position) > 1f && Vector3.Distance(player.transform.position, arrow.transform.position) <= 1f)
            {
                player.GetComponent<Archer>().enabled = true;
                Destroy(sword);
                Destroy(arrow);
                stageProgression++;
            }
        }
    }

    void OpenDoor(int doorNo)
    {
        doors.transform.GetChild(doorNo).GetComponent<DoorScript>().Open();
    }

    void CloseDoor(int doorNo)
    {
        doors.transform.GetChild(doorNo).GetComponent<DoorScript>().Close();
    }

    public void Restart()
    {
        SceneManager.LoadScene("Tempo");
    }
}
