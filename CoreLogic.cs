using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoreLogic : MonoBehaviour
{
    protected int length;
    protected int height;
    protected int prev_index = -1;
    protected int transitional_index = -1;
    protected int link_index1 = -1;
    protected int link_index2 = -1;
    protected int true_link_index = -1;
    protected int counter = 0;
    protected bool execution = false;
    protected GameObject pressed_button = null;
    protected GameObject hint_button1 = null;
    protected GameObject hint_button2 = null;
    protected GameObject load_bar = null;
    protected GameObject path1 = null;
    protected GameObject path2 = null;
    protected GameObject path3 = null;
    protected Color stage1 = new Color (0.5f,  0.5f,  0.5f);
    protected Color stage2 = new Color(0.3f, 0.3f, 0.3f);
    protected Color stage3 = new Color(1, 1, 1);
    protected List<Data> index_and_images;
    protected enum Twinkle { primitive, step1, step2, step3, step4, step5, step6 };
    protected Twinkle twinkle_counter = Twinkle.primitive;




    // Use this for initialization
    void Awake()
    {

    }
    void Start ()
    {
        var data = new Data();
        length = data.Length;
        height = data.Height;
        index_and_images = Coord_information.create_list();
        var background = this.transform.Find("background");
        load_bar = Resources.Load<GameObject>("bar");
        for (int i = 0; i < index_and_images.Count; i++)
        {
            int index = i;
            if (index_and_images[i].Category == 0)
            {
                index_and_images[i].Status = false;
            }
            else
            {
                var load_button = Resources.Load<GameObject>("Choice");
                var initialize_button = GameObject.Instantiate(load_button);
                initialize_button.transform.SetParent(background);
                float x = -260 + (i % 12) * 50;
                float y = -270 + (i / 12) * 50;
                initialize_button.transform.localPosition = new Vector3(x, y);
                initialize_button.transform.localScale = Vector3.one;
                initialize_button.transform.name = i.ToString();
                initialize_button.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>(index_and_images[i].Category.ToString());
                initialize_button.GetComponent<Button>().onClick.AddListener(() => { this.Clicked(index, initialize_button); });
            }
        }
        this.transform.Find("hint").GetComponent<Button>().onClick.AddListener(() => { this.Hint(); });
    }
	
	// Update is called once per frame
	void Update ()
    {
        switch (this.twinkle_counter)
        {
            case Twinkle.primitive:
                {

                }
                break;
            case Twinkle.step1:
                {
                    hint_button1.GetComponent<Image>().color = stage2;
                    hint_button2.GetComponent<Image>().color = stage2;
                    this.twinkle_counter = Twinkle.step2;
                }
                break;
            case Twinkle.step2:
                {
                    hint_button1.GetComponent<Image>().color = stage1;
                    hint_button2.GetComponent<Image>().color = stage1;
                    this.twinkle_counter = Twinkle.step3;
                }
                break;
            case Twinkle.step3:
                {
                    hint_button1.GetComponent<Image>().color = stage2;
                    hint_button2.GetComponent<Image>().color = stage2;
                    this.twinkle_counter = Twinkle.step4;
                }
                break;
            case Twinkle.step4:
                {
                    hint_button1.GetComponent<Image>().color = stage1;
                    hint_button2.GetComponent<Image>().color = stage1;
                    this.twinkle_counter = Twinkle.step5;
                }
                break;
            case Twinkle.step5:
                {
                    hint_button1.GetComponent<Image>().color = stage2;
                    hint_button2.GetComponent<Image>().color = stage2;
                    this.twinkle_counter = Twinkle.step6;
                }
                break;
            case Twinkle.step6:
                {
                    hint_button1.GetComponent<Image>().color = stage1;
                    hint_button2.GetComponent<Image>().color = stage1;
                    this.twinkle_counter = Twinkle.step1;
                }
                break;
        }
        if (path1 != null || path2 != null || path3 != null)
        {
            counter = counter + 1;
            if (counter == 100)
            {
                counter = 0;
                if(path1 != null)
                {
                    path1.gameObject.SetActive(false);
                }
                if (path2 != null)
                {
                    path2.gameObject.SetActive(false);
                }
                if (path3 != null)
                {
                    path3.gameObject.SetActive(false);
                }
            }
        }
		
	}

    public void Clicked(int index, GameObject button)
    {
        bool Status;
        if (prev_index == -1)
        {
            prev_index = index;
            pressed_button = button;
            return;
        }
        // judge whether they are the same button or they are different kinds.
        else if (pressed_button == button || index_and_images[prev_index].Category != index_and_images[index].Category)
        {
            this.Reset_data();
            return;
        }
        else
        {
            Status = this.Final_elimate(index);
        }
        if (Status == true)
        {
            this.Hide_button(index, prev_index, button, pressed_button);
            this.twinkle_counter = Twinkle.primitive;
            this.Reset_data();
        }
        else
        {
            this.Reset_data();
        }
    }

    public void Hint()
    {
        if(hint_button1 != null && hint_button2 != null)
        {
            hint_button1.GetComponent<Image>().color = stage3;
            hint_button2.GetComponent<Image>().color = stage3;
        }
        this.Reset_data();
        List<int> hints_counter = new List<int>();
        for(int i = 0; i < index_and_images.Count; i++)
        {
            if(index_and_images[i].Status == true)
            {
                hints_counter.Add(i);
            }
        }
        int random_index = hints_counter[UnityEngine.Random.Range(0, hints_counter.Count + 1)];
        for (int i1 = random_index; i1 < height * length; i1++)
        {
            prev_index = i1;
            if (index_and_images[prev_index].Status == true)
            {
                for (int i2 = prev_index + 1; i2 < height * length + 1; i2++)
                {
                    if (index_and_images[i2].Status == true && index_and_images[prev_index].Category == index_and_images[i2].Category)
                    {
                        bool Status;
                        Status = this.Final_elimate(i2);
                        if (Status == true)
                        {
                            hint_button1 = this.transform.Find("background/" + prev_index.ToString()).gameObject;
                            hint_button2 = this.transform.Find("background/" + i2.ToString()).gameObject;
                            this.twinkle_counter = Twinkle.step1;
                            this.Reset_data();
                            return;
                        }
                    }
                }
            }
        }
        for (int i1 = random_index; i1 > 1; i1--)
        {
            prev_index = i1;
            if (index_and_images[prev_index].Status == true)
            {
                for (int i2 = prev_index - 1; i2 > 0; i2--)
                {
                    if (index_and_images[i2].Status == true && index_and_images[prev_index].Category == index_and_images[i2].Category)
                    {
                        bool Status;
                        Status = this.Final_elimate(i2);
                        if (Status == true)
                        {
                            hint_button1 = this.transform.Find("background/" + prev_index.ToString()).gameObject;
                            hint_button2 = this.transform.Find("background/" + i2.ToString()).gameObject;
                            this.twinkle_counter = Twinkle.step1;
                            this.Reset_data();
                            return;
                        }
                    }
                }
            }
        }
        this.Reset_data();
        Debug.Log("No more hints!");
    }

    public bool Existance(int x1,int x2,int y1 ,int y2)
    {
        var index1 = Find_coord(x1, y2);
        var index2 = Find_coord(x2, y1);
        if (index_and_images[index1].Status == false)
        {
            link_index1 = index1;
        }
        if (index_and_images[index2].Status == false)
        {
            link_index2 = index2;
        }
        if(index_and_images[index1].Status == false || index_and_images[index2].Status == false)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Elimate(int vert, int hori, int index,int prev_index)
    {
        //close to each other
        if ((Mathf.Abs(vert - index_and_images[index].Vert) == 1 && hori == index_and_images[index].Hori) || (Mathf.Abs(hori - index_and_images[index].Hori) == 1 && vert == index_and_images[index].Vert))
        {
            return true;
        }
        //vertically far away
        else if (hori == index_and_images[index].Hori)
        {
            if (vert < index_and_images[index].Vert)
            {
                for (int i = prev_index + height + 2; i < index; i = i + height + 2)
                {
                    var judgement = index_and_images[i].Status;
                    if (judgement == true)
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                for (int i = index + height + 2; i < prev_index; i = i + height + 2)
                {
                    var judgement = index_and_images[i].Status;
                    if (judgement == true)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        //horizontally far away
        else if (vert == index_and_images[index].Vert)
        {
            if (hori < index_and_images[index].Hori)
            {
                for (int i = prev_index + 1; i < index; i++)
                {
                    var judgement = index_and_images[i].Status;
                    if (judgement == true)
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                for (int i = index + 1; i < prev_index; i++)
                {
                    var judgement = index_and_images[i].Status;
                    if (judgement == true)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        else
        {
            return false;
        }
    }

    public bool Sub_elimate(int vertical, int horizontal , int index , int prev_index, int link_index1, int link_index2)
    {
        bool status = false;
        if (link_index1 != -1)
        {
            status = this.Elimate(vertical, horizontal, link_index1, prev_index);
            if (status == true)
            {
                status = this.Elimate(index_and_images[index].Vert, index_and_images[index].Hori, link_index1, index);
                if (status == true)
                {
                    true_link_index = link_index1;
                    link_index1 = -1;
                    return true;
                }
            }
        }
        if (link_index2 != -1)
        {
            status = this.Elimate(vertical, horizontal, link_index2, prev_index);
            if (status == true)
            {
                status = this.Elimate(index_and_images[index].Vert, index_and_images[index].Hori, link_index2, index);
                if (status == true)
                {
                    true_link_index = link_index2;
                    link_index2 = -1;
                    return true;
                }
            }
        }
        true_link_index = -1;
        link_index1 = -1;
        link_index2 = -1;
        return false;
    }

    public bool Final_elimate(int index)
    {
        //close to each otehr
        if (index_and_images[prev_index].Vert == index_and_images[index].Vert || index_and_images[prev_index].Hori == index_and_images[index].Hori)
        {
            execution = this.Elimate(index_and_images[prev_index].Vert, index_and_images[prev_index].Hori, index, prev_index);
            if (execution == true)
            {
                return true;
            }
        }
        // one link point
        if (Existance(index_and_images[prev_index].Hori, index_and_images[index].Hori, index_and_images[prev_index].Vert, index_and_images[index].Vert) == true)
        {
            execution = Sub_elimate(index_and_images[prev_index].Vert, index_and_images[prev_index].Hori, index, prev_index, link_index1, link_index2);
            if (execution == true)
            {
                return true;
            }
        }
        //two link points
        //vertically increase(Up) search
        for (int i = index_and_images[prev_index].Vert + 1; i < height + 2; i++)
        {
            if (index_and_images[this.Find_coord(index_and_images[prev_index].Hori, i)].Status == true)
            {
                break;
            }
            if (index_and_images[this.Find_coord(index_and_images[prev_index].Hori, i)].Status == false)
            {
                if (Existance(index_and_images[prev_index].Hori, index_and_images[index].Hori, i, index_and_images[index].Vert) == true)
                {
                    execution = Sub_elimate(index_and_images[index].Vert, index_and_images[index].Hori, this.Find_coord(index_and_images[prev_index].Hori, i), index, link_index1, link_index2);
                    if (execution == true)
                    {
                        transitional_index = this.Find_coord(index_and_images[prev_index].Hori,i);
                        return true;
                    }
                }
            }
        }
        //vertically decrease(Down) search
        for (int i = index_and_images[prev_index].Vert - 1; i > -1; i--)
        {
            if (index_and_images[this.Find_coord(index_and_images[prev_index].Hori, i)].Status == true)
            {
                break;
            }
            if (index_and_images[this.Find_coord(index_and_images[prev_index].Hori, i)].Status == false)
            {
                if (Existance(index_and_images[prev_index].Hori, index_and_images[index].Hori, i, index_and_images[index].Vert) == true)
                {
                    execution = Sub_elimate(index_and_images[index].Vert, index_and_images[index].Hori, this.Find_coord(index_and_images[prev_index].Hori, i), index, link_index1, link_index2);
                    if (execution == true)
                    {
                        transitional_index = this.Find_coord(index_and_images[prev_index].Hori, i);
                        return true;
                    }
                }
            }
        }
        //horizontally increase(Right) search
        for (int i = index_and_images[prev_index].Hori + 1; i < length + 2; i++)
        {
            if (index_and_images[this.Find_coord(i, index_and_images[prev_index].Vert)].Status == true)
            {
                break;
            }
            if (index_and_images[this.Find_coord(i, index_and_images[prev_index].Vert)].Status == false)
            {
                if (Existance(i, index_and_images[index].Hori, index_and_images[prev_index].Vert, index_and_images[index].Vert) == true)
                {
                    execution = Sub_elimate(index_and_images[index].Vert, index_and_images[index].Hori, this.Find_coord(i, index_and_images[prev_index].Vert), index, link_index1, link_index2);
                    if (execution == true)
                    {
                        transitional_index = this.Find_coord(i, index_and_images[prev_index].Vert);
                        return true;
                    }
                }
            }
        }
        //horizontally decrease(Left) search
        for (int i = index_and_images[prev_index].Hori - 1; i > -1; i--)
        {
            if (index_and_images[this.Find_coord(i, index_and_images[prev_index].Vert)].Status == true)
            {
                break;
            }
            if (index_and_images[this.Find_coord(i, index_and_images[prev_index].Vert)].Status == false)
            {
                if (Existance(i, index_and_images[index].Hori, index_and_images[prev_index].Vert, index_and_images[index].Vert) == true)
                {
                    execution = Sub_elimate(index_and_images[index].Vert, index_and_images[index].Hori, this.Find_coord(i, index_and_images[prev_index].Vert), index, link_index1, link_index2);
                    if (execution == true)
                    {
                        transitional_index = this.Find_coord(i, index_and_images[prev_index].Vert);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void Hide_button(int index, int prev_index,GameObject button, GameObject pressed_button)
    {
        if (transitional_index != -1)
        {
            path3 = Find_path(transitional_index, prev_index);
            path2 = Find_path(true_link_index, transitional_index);
            path1 = Find_path(true_link_index, index);
        }
        else if (true_link_index != -1)
        {
            path2 = Find_path(true_link_index, prev_index);
            path1 = Find_path(true_link_index, index);
        }
        else
        {
            path1 = Find_path(index, prev_index);
        }
        button.gameObject.SetActive(false);
        pressed_button.gameObject.SetActive(false);
        index_and_images[index].Status = false;
        index_and_images[prev_index].Status = false;
        this.Reset_data();
    }

    public int Find_coord(int x, int y)
    {
        return (x + y * (length + 2));
    }

    public GameObject Find_path(int index, int prev_index)
    {
        var background = this.transform.Find("background");
        var initialize_bar = GameObject.Instantiate(load_bar);
        initialize_bar.transform.SetParent(background);
        if (index_and_images[prev_index].Vert == index_and_images[index].Vert)
        {
            initialize_bar.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Abs((float)index_and_images[prev_index].Hori - index_and_images[index].Hori) * 50, 5);
            if ((-260 + (index % 12) * 50 < 0 && -260 + (prev_index % 12) * 50 > 0) || (-260 + (index % 12) * 50 > 0 && -260 + (prev_index % 12) * 50 < 0))
            {
                if (-260 + (index % 12) * 50 < 0)
                {
                    float x = -260 + (index % 12) * 50 + (Mathf.Abs(-260 + (index % 12) * 50) + Mathf.Abs(-260 + (prev_index % 12) * 50)) / 2;
                    float y = -270 + (index / 12) * 50;
                    initialize_bar.transform.localPosition = new Vector3(x, y);
                    initialize_bar.transform.localScale = Vector3.one;
                    return initialize_bar;
                }
                else
                {
                    float x = -260 + (prev_index % 12) * 50 + (Mathf.Abs(-260 + (index % 12) * 50) + Mathf.Abs(-260 + (prev_index % 12) * 50)) / 2;
                    float y = -270 + (index / 12) * 50;
                    initialize_bar.transform.localPosition = new Vector3(x, y);
                    initialize_bar.transform.localScale = Vector3.one;
                    return initialize_bar;
                }
            }
            else
            {
                float x = ((-260 + (prev_index % 12) * 50) + (-260 + (index % 12) * 50)) / 2;
                float y = -270 + (index / 12) * 50;
                initialize_bar.transform.localPosition = new Vector3(x, y);
                initialize_bar.transform.localScale = Vector3.one;
                return initialize_bar;
            }
        }
        else
        {
            initialize_bar.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(5, Mathf.Abs((float)index_and_images[prev_index].Vert - index_and_images[index].Vert) * 50);
            if ((-270 + (index / 12) * 50 < 0 && -270 + (prev_index / 12) * 50 > 0) || -270 + (index / 12) * 50 > 0 && -270 + (prev_index / 12) * 50 < 0)
            {
                if (-270 + (index / 12) * 50 < 0)
                {
                    float y = -270 + (index / 12) * 50 + (Mathf.Abs(-270 + (index / 12) * 50) + Mathf.Abs(-270 + (prev_index / 12) * 50)) / 2;
                    float x = -260 + (index % 12) * 50;
                    initialize_bar.transform.localPosition = new Vector3(x, y);
                    initialize_bar.transform.localScale = Vector3.one;
                    return initialize_bar;
                }
                else
                {
                    float y = -270 + (prev_index / 12) * 50 + (Mathf.Abs(-270 + (index / 12) * 50) + Mathf.Abs(-270 + (prev_index / 12) * 50)) / 2;
                    float x = -260 + (index % 12) * 50;
                    initialize_bar.transform.localPosition = new Vector3(x, y);
                    initialize_bar.transform.localScale = Vector3.one;
                    return initialize_bar;
                }
            }
            else
            {
                float y = ((-270 + (prev_index / 12) * 50) + (-270 + (index / 12) * 50)) / 2;
                float x = -260 + (index % 12) * 50;
                initialize_bar.transform.localPosition = new Vector3(x, y);
                initialize_bar.transform.localScale = Vector3.one;
                return initialize_bar;
            }
        }
    }
 
    public void Reset_data()
    {
        prev_index = -1;
        link_index1 = -1;
        link_index2 = -1;
        true_link_index = -1;
        transitional_index = -1;
        pressed_button = null;
        execution = false;

    }
}

public class Data
{
    private List<Data> index_and_images = new List<Data>();
    private int length = 10;
    private int height = 10;
    protected int vert = 0;
    protected int hori = 0;
    protected bool status = true;
    protected int category = 0;
    protected int index = 0;

    public int Length
    {
        get
        {
            return length;
        }
    }

    public int Height
    {
        get
        {
            return height;
        }
    }

    public int Vert
    {
        get
        {
            return vert;
        }
        set
        {
            vert = value;
        }
    }

    public int Hori
    {
        get
        {
            return hori;
        }
        set
        {
            hori = value;
        }
    }

    public bool Status
    {
        get
        {
            return status;
        }
        set
        {
            status = value;
        }
    }

    public int Category
    {
        get
        {
            return category;
        }
        set
        {
            category = value;
        }
    }

    public int Index
    {
        get
        {
            return index;
        }
        set
        {
            index = value;
        }
    }

    public List<Data> Index_and_images
    {
        get
        {
            return index_and_images;
        }
    }
}

public class Coord_information
{
    public static List<Data> create_list()
    {
        var data = new Data();
        List<int> counter = new List<int>();
        for (int i1 = 0; i1 < (data.Height + 2) * (data.Length + 2); i1++)
        {
            var content = new Data();
            content.Index = i1;
            content.Vert = i1 / 12;
            content.Hori = i1 % 12;
            data.Index_and_images.Add(content);
        }
        for (int i3 = 0; i3 < (data.Height + 2) * (data.Length + 2); i3++)
        {
            if ((data.Index_and_images[i3].Vert != 0) && (data.Index_and_images[i3].Vert != data.Length + 1) && (data.Index_and_images[i3].Hori != 0) && (data.Index_and_images[i3].Hori != data.Height + 1))
            {
                counter.Add(data.Index_and_images[i3].Index);
            }
        }
        for (int i4 = 0; i4 < data.Length * data.Height / 2; i4++)
        {
            // which image
            int random_image = UnityEngine.Random.Range(1, 11);
            //first time
            int random_index = UnityEngine.Random.Range(0, counter.Count);
            data.Index_and_images[counter[random_index]].Category = random_image;
            counter.RemoveAt(random_index);
            //second time
            random_index = UnityEngine.Random.Range(0, counter.Count);;
            data.Index_and_images[counter[random_index]].Category = random_image;
            counter.RemoveAt(random_index);
        }
        return data.Index_and_images;
    }
}