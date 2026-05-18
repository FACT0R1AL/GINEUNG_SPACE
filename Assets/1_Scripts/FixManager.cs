using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum FixType
{
    Engine,
    Wall,
    Oxygen,
    Drone
}

public class FixManager : MonoBehaviour
{
    public bool brokenEngine;
    public bool brokenWall;
    public bool brokenOxygen;
    public bool brokenDrone;

    public Color fixColor;
    public Color brokenColor;

    public Image engineImage;
    public Image wallImage;
    public Image oxygenImage;
    public Image droneImage;

    public FixType fix;


    public void broken(FixType fix)
    {
        switch (fix)
        {
            case FixType.Engine:
                brokenEngine = true;
                engineImage.color = brokenColor;
                break;
            case FixType.Wall:
                brokenWall = true;
                wallImage.color = brokenColor;
                break;
            case FixType.Oxygen:
                brokenOxygen = true;
                oxygenImage.color = brokenColor;
                break;
            case FixType.Drone:
                brokenDrone = true;
                droneImage.color = brokenColor;
                break;
        }
    }

    private void Update()
    {
        foreach (var fixType in System.Enum.GetValues(typeof(FixType)))
        {
            switch ((FixType)fixType)
            {
                case FixType.Engine:
                    if (brokenEngine)
                    {
                        Broking(engineImage);
                    }
                    else
                    {
                        engineImage.color = fixColor;
                    }
                        break;
                case FixType.Wall:
                    if (brokenWall)
                    {
                        Broking(wallImage);
                    }
                    else
                    {
                        wallImage.color = fixColor;
                    }
                        break;
                case FixType.Oxygen:
                    if (brokenOxygen)
                    {
                        Broking(oxygenImage);
                    }
                    else
                    {
                        oxygenImage.color = fixColor;
                    }
                        break;
                case FixType.Drone:
                    if (brokenDrone)
                    {
                        Broking(droneImage);
                    }
                    else
                    {
                        droneImage.color = fixColor;
                    }
                        break;
            }
        }
    }

    private void Broking(Image image)
    {
        Color color = image.color;
        color.r = brokenColor.r;
        color.g = brokenColor.g;
        color.b = brokenColor.b;
        //float alpha = color.a;

        //if (alpha < .5)
        //{
        //    alpha += Time.deltaTime;
        //}
        //else
        //{
        //    alpha -= Time.deltaTime;
        //}
        //color.a = alpha;
        float speed = 1.0f;
        color.a = Mathf.PingPong(Time.time * speed, 1f);
        image.color = color;
    }
}
