using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AvatarSys : MonoBehaviour
{
    public static AvatarSys _instance;

    private GameObject girlSource;// 资源model
    private Transform girlSourceTrans;
    private GameObject girlTarget;// 骨架物体，换到别人身上
    private Dictionary<string, Dictionary<string, SkinnedMeshRenderer>> girlData = new Dictionary<string, Dictionary<string, SkinnedMeshRenderer>>();
    // 小女孩所有的资源信息
    Transform[] girlHips;// 小女孩骨骼信息
    private Dictionary<string, SkinnedMeshRenderer> girlSmr = new Dictionary<string, SkinnedMeshRenderer>();// 换装骨骼身上的skm信息
    private string[,] girlStr = new string[,] { { "eyes", "1" }, { "hair", "1" }, { "face", "1" }, { "top", "1" }, { "face", "1" }, { "pants", "1" }, { "shoes", "1" } };

    private GameObject boySource;// 资源model
    private Transform boySourceTrans;
    private GameObject boyTarget;// 骨架物体，换到别人身上
    private Dictionary<string, Dictionary<string, SkinnedMeshRenderer>> boyData = new Dictionary<string, Dictionary<string, SkinnedMeshRenderer>>();
    // 小女孩所有的资源信息
    Transform[] boyHips;// 小女孩骨骼信息
    private Dictionary<string, SkinnedMeshRenderer> boySmr = new Dictionary<string, SkinnedMeshRenderer>();// 换装骨骼身上的skm信息
    private string[,] boyStr = new string[,] { { "eyes", "1" }, { "hair", "1" }, { "face", "1" }, { "top", "1" }, { "face", "1" }, { "pants", "1" }, { "shoes", "1" } };

    public int nowCount = 0;// 0代表小女孩 ,1 代表小男孩



    public GameObject girlPanel;
    public GameObject boyPanel;

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        GirlAvatar();
        BoyAvatar();
        //boyTarget.SetActive(true);
        girlTarget.AddComponent<SpinWithMouse>();
        boyTarget.AddComponent<SpinWithMouse>();
        boyTarget.SetActive(false);

    }

    public void GirlAvatar()
    {
        InstantiateGirl();
        SaveData(girlSourceTrans,girlData,girlTarget,girlSmr);
        InitAvatarGirl();
    }

    public void BoyAvatar()
    {
        InstantiateBoy();
        SaveData(boySourceTrans, boyData, boyTarget, boySmr);
        InitAvatarBoy();

    }

    //void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        int num = Random.Range(1, 7);
    //        ChanageMeshGirl("top", num.ToString());
    //    }
    //}

    void InstantiateGirl()
    {
        girlSource = Instantiate(Resources.Load("FemaleModel")) as GameObject;
        girlSourceTrans = girlSource.transform;
        girlSource.SetActive(false);

        girlTarget = Instantiate(Resources.Load("FemaleTarget")) as GameObject;
        
        //girlTarget.SetActive(false);
        girlHips = girlTarget.GetComponentsInChildren<Transform>();
    }

    void InstantiateBoy()
    {
        boySource = Instantiate(Resources.Load("MaleModel")) as GameObject;
        boySourceTrans = boySource.transform;
        boySource.SetActive(false);

        boyTarget = Instantiate(Resources.Load("MaleTarget")) as GameObject;
        
        
        boyHips = boyTarget.GetComponentsInChildren<Transform>();
    }


    void SaveData(Transform sourceTrans, Dictionary<string, Dictionary<string, SkinnedMeshRenderer>> data, GameObject target,
        Dictionary<string, SkinnedMeshRenderer> smr)
    {
        data.Clear();
        smr.Clear();
        if (sourceTrans == null)
        {
            return;
        }
        
        SkinnedMeshRenderer[] parts = sourceTrans.GetComponentsInChildren<SkinnedMeshRenderer>();//遍历所有节点获取SkinnedMeshRenderer节点
        foreach (var part in parts)
        {
            string[] names = part.name.Split('-');
            if (!data.ContainsKey(names[0]))
            {
                // 生成对应的部位,且只生产一个
                GameObject partGo = new GameObject();
                partGo.name = names[0];
                partGo.transform.parent = target.transform;

                smr.Add(names[0], partGo.AddComponent<SkinnedMeshRenderer>());
                data.Add(names[0], new Dictionary<string, SkinnedMeshRenderer>());
            }
            data[names[0]].Add(names[1], part);//存储所有的skinnedRender信息到数据里面


        }
    }

    void ChanageMeshGirl(string part, string num)
    { // 传入部位,编号 从data里面拿去对应ske
        SkinnedMeshRenderer skm = girlData[part][num];// 要更好的部位
        List<Transform> bones = new List<Transform>();
        foreach (var trans in skm.bones)
        {
            foreach (var bone in girlHips)
            {
                if (bone.name == trans.name)
                {
                    bones.Add(bone);
                    break;
                }
            }
        }
        // 换装实现
        girlSmr[part].bones = bones.ToArray();
        girlSmr[part].material = skm.material;
        girlSmr[part].sharedMesh = skm.sharedMesh;

        SaveDat(part, num, girlStr);
    }

    void ChanageMeshBoy(string part, string num)
    { // 传入部位,编号 从data里面拿去对应ske
        SkinnedMeshRenderer skm = boyData[part][num];// 要更好的部位
        List<Transform> bones = new List<Transform>();
        foreach (var trans in skm.bones)
        {
            foreach (var bone in boyHips)
            {
                if (bone.name == trans.name)
                {
                    bones.Add(bone);
                    break;
                }
            }
        }
        // 换装实现
        boySmr[part].bones = bones.ToArray();
        boySmr[part].material = skm.material;
        boySmr[part].sharedMesh = skm.sharedMesh;

        SaveDat(part, num,boyStr);
    }

    void InitAvatarGirl()
    { // 初始化骨骼让他有mash材质 骨骼信息
        int length = girlStr.GetLength(0);// 获取行数
        for (int i = 0; i < length; i++)
        {
            ChanageMeshGirl(girlStr[i, 0], girlStr[i, 1]);
        }
    }

    void InitAvatarBoy()
    { // 初始化骨骼让他有mash材质 骨骼信息
        int length = boyStr.GetLength(0);// 获取行数
        for (int i = 0; i < length; i++)
        {
            ChanageMeshBoy(boyStr[i, 0], boyStr[i, 1]);
        }
       
    }

    public void onChangePeople(string part,string num) {
        if (nowCount == 0) {
            ChanageMeshGirl(part, num);
        }
        else
        {
            ChanageMeshBoy(part, num);
        }
    }

    public void SexChange() { // 性别切换 任务隐藏 面板切换
        if(nowCount == 0)
        {
            nowCount = 1;
            girlTarget.SetActive(false);
            boyTarget.SetActive(true);
            girlPanel.SetActive(false);
            boyPanel.SetActive(true);

        }else{
            nowCount = 0;
            girlTarget.SetActive(true);
            boyTarget.SetActive(false);
            girlPanel.SetActive(true);
            boyPanel.SetActive(false);
         
        }
    }

    public void SaveDat(string part,string num,string[,] str) {
        int length = girlStr.GetLength(0);
        for (int i = 0; i < length; i++) {
            if (str[i, 0] == part) {
                str[i, 1] = num;
            }
        }
    }

    public void LoadScenes() {
        SceneManager.LoadScene(1);
    }
}
