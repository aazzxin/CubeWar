using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Loading : MonoBehaviour {
    private float fps = 10.0f;
    private float time;
    //一组动画的贴图，在编辑器中赋值。
    //public Texture2D[] animations;
    //private int nowFram;
    [SerializeField]
    private Image loadImage;
    //异步对象
    AsyncOperation async;
 
    //读取场景的进度，它的取值范围在0 - 1 之间。
    private int progress = 0;
    private bool isLoaded = false;//完成时为true
    void Start()
    {
        //在这里开启一个异步任务，
        //进入loadScene方法。
        StartCoroutine(loadScene());
    }
    //注意这里返回值一定是 IEnumerator
    IEnumerator loadScene()
    {
        int displayProgress = 0;
        int toProgress = 0;

        //异步读取场景。
        //Globe.loadName 就是A场景中需要读取的C场景名称。
        async = Application.LoadLevelAsync(Globe.loadName);
        async.allowSceneActivation = false;

        while (async.progress < 0.9f)
        {
            toProgress = (int)async.progress * 100;
            while (displayProgress < toProgress)
            {
                ++displayProgress;
                SetLoadingPercentage(displayProgress);
                yield return new WaitForEndOfFrame();
            }
        }

        toProgress = 100;
        while (displayProgress < toProgress)
        {
            ++displayProgress;
            SetLoadingPercentage(displayProgress);
            yield return new WaitForEndOfFrame();
        }
        //读取完毕后返回， 系统会自动进入C场景

        async.allowSceneActivation = true;

        MainGameManager.playerManager.SetPlayerTransform();
        
    }
 
    //void OnGUI()
    //{
    //    //因为在异步读取场景，
    //    //所以这里我们可以刷新UI
    //    DrawAnimation(animations);
 
    //}
 
    void Update()
    {
        
    }

    void SetLoadingPercentage(float index)
    {
        progress = (int)index;
        loadImage.rectTransform.localScale = new Vector3((float)(progress) / 100, loadImage.rectTransform.localScale.y, loadImage.rectTransform.localScale.z);
    }
    //这是一个简单绘制2D动画的方法，没什么好说的。
    //void DrawAnimation(Texture2D[] tex)
    //{

    //    time += Time.deltaTime;

    //     if(time >= 1.0 / fps){

    //         nowFram++;

    //         time = 0;

    //         if(nowFram >= tex.Length)
    //         {
    //            nowFram = 0;
    //         }
    //    }
    //    GUI.DrawTexture(new Rect( 100,100,40,60) ,tex[nowFram] );

    //    //在这里显示读取的进度。
    //    GUI.Label(new Rect( 100,180,300,60), "lOADING!!!!!" + progress);

    //}
}
