using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 
/// </summary>
public class Store : MonoBehaviour
{
    public static critical_information user_data = new critical_information();

    // Use this for initialization
    private void Start()
    {
        GameObject initializeInUnity;
        var result = ProductInformation.getContent();
        var figure = CoinsInformation.getContent();
        this.transform.Find("toolShop/mainTittle/StateBar/Button").GetComponent<Button>().onClick.AddListener(() => { this.reset(); });
        this.transform.Find("coinShop/switch").GetComponent<Button>().onClick.AddListener(() => { this.switcher1(); });
        this.transform.Find("toolShop/switch").GetComponent<Button>().onClick.AddListener(() => { this.switcher2(); });
        var scrollView = this.transform.Find("coinShop/mainTittle/scrollView").GetComponent<ScrollRect>();
        var initializeBlue = Resources.Load<GameObject>("BlueBar");
        var initializeYellow = Resources.Load<GameObject>("YellowBar");
        for (int i = 0; i < result.Count; i++)
        {
            if ((i == 0) || (i == 2))
            {
                initializeInUnity = GameObject.Instantiate(initializeYellow);
            }
            else
            {
                initializeInUnity = GameObject.Instantiate(initializeBlue);
            }
            initializeInUnity.transform.SetParent(scrollView.content);
            initializeInUnity.transform.localPosition = Vector3.zero;
            initializeInUnity.transform.localScale = Vector3.one;
            var detailedData = result[i];
            var test = Resources.Load<Sprite>("CoinImage" + i.ToString());
            initializeInUnity.transform.Find("CoinImage").GetComponent<Image>().sprite = test;
            initializeInUnity.transform.Find("Button").GetComponent<Button>().onClick.AddListener(() => { this.clicked(detailedData); });
            initializeInUnity.transform.Find("Button/ButtonText").GetComponent<Text>().text = "BUY";
            initializeInUnity.transform.Find("Price").GetComponent<Text>().text = (result[i].Price).ToString();
            initializeInUnity.transform.Find("Coins").GetComponent<Text>().text = (result[i].Coins).ToString();
            initializeInUnity.transform.Find("Discounts").GetComponent<Text>().text = (result[i].Discount).ToString();
        }
        for (int i = 0; i < 4; i++)
        {
            var index = i.ToString();
            var toolsData = figure[i];
            this.transform.Find("toolShop/mainTittle/choice" + index + "/numberOfGreen").GetComponent<Text>().text = (figure[i].Green).ToString();
            this.transform.Find("toolShop/mainTittle/choice" + index + "/numberOfYellow").GetComponent<Text>().text = (figure[i].Yellow).ToString();
            this.transform.Find("toolShop/mainTittle/choice" + index + "/numberOfRed").GetComponent<Text>().text = (figure[i].Red).ToString();
            this.transform.Find("toolShop/mainTittle/choice" + index + "/Price").GetComponent<Text>().text = (figure[i].Price).ToString();
            this.transform.Find("toolShop/mainTittle/choice" + index + "/Button").GetComponent<Button>().onClick.AddListener(() => { this.refresh(toolsData); });
        }
    }

    protected void refresh(TestData figure)
    {
        var last_number = 0;
        var order = new Dictionary<int, List<int>>();
        order.Add(1, new List<int>{ 0, 1 });
        order.Add(2, new List<int> { 2, 3 });
        order.Add(3, new List<int> { 4, 5, 6 });
        var catalog = new Dictionary<int, int>();
        catalog.Add(0, figure.Green);
        catalog.Add(1, figure.Yellow);
        catalog.Add(2, figure.Red);
        for (int i1 = 1; i1 < 4; i1++)
        {
            int parameter = 0;
            var Cata = catalog[i1-1];
            var output = order[i1];
            for (int n = 0; n < output.Count - 1; n++)
            {
                var result = output[n];
                var ran = UnityEngine.Random.Range(0, Cata - parameter);
                user_data.revise(result, ran);
                parameter = ran + parameter;
                last_number = Cata - parameter;
            }
            user_data.revise(output[output.Count-1],last_number);
        }
        for (int i = 0; i < user_data.count() ; i++)
        {
            this.transform.Find("toolShop/mainTittle/StateBar/" + i).GetComponent<Text>().text = (user_data.getback()[i]).ToString();
        }
        user_data.Times = user_data.Times + 1;
        this.transform.Find("toolShop/mainTittle/StateBar/times").GetComponent<Text>().text = (user_data.Times).ToString();
    }

    protected void clicked(detailInformation data)
    {
        Debug.Log("You can spend $" + (data.Price).ToString() + "and you can get " + (data.Coins).ToString() + " coins. The discount you have is : " + (data.Discount).ToString());
    }

    protected void reset()
    {
        user_data.reset();
        user_data.times_reset();
        this.transform.Find("toolShop/mainTittle/StateBar/times").GetComponent<Text>().text = (user_data.Times).ToString();
        for (int i = 0; i < user_data.count();i++)
        {
            this.transform.Find("toolShop/mainTittle/StateBar/" + i).GetComponent<Text>().text = (user_data.getback()[i]).ToString();
        }
    }

    protected void switcher1()
    {
        var coinshop = this.transform.Find("coinShop");
        var toolshop = this.transform.Find("toolShop");
        coinshop.gameObject.SetActive(false);
        toolshop.gameObject.SetActive(true);
    }

    protected void switcher2()
    {
        var coinshop = this.transform.Find("coinShop");
        var toolshop = this.transform.Find("toolShop");
        coinshop.gameObject.SetActive(true);
        toolshop.gameObject.SetActive(false);
    }
}
public class detailInformation
{
    protected int price = 0;
    protected int coins = 0;
    protected float discount = 0f;

    public int Price
    {
        get
        {
            return price;
        }
        set
        {
            price = value;
        }
    }
    public int Coins
    {
        get
        {
            return coins;
        }
        set
        {
            coins = value;
        }
    }
    public float Discount
    {
        get
        {
            return discount;
        }
        set
        {
            discount = value;
        }
    }
}

public class ProductInformation
{ 
        public static List<detailInformation> getContent()
        {
            var listContent = new List<detailInformation>();
            for (int i = 0; i < 6; i++)
            {
                var goodsInformation = new detailInformation();
                goodsInformation.Price = (1 + i) * i;
                goodsInformation.Coins = (1 + i) * 4;
                goodsInformation.Discount = i + 1;
                listContent.Add(goodsInformation);
            }
            return listContent;
        }
}

public class TestData
{
    protected int green = 0;
    protected int red = 0;
    protected int yellow = 0;
    protected float price = 0f;

    public int Green
    {
        get
        {
            return green;
        }
        set
        {
            green = value;
        }
    }
    public int Red
    {
        get
        {
            return red;
        }
        set
        {
            red = value;
        }
    }
    public int Yellow
    {
        get
        {
            return yellow;
        }
        set
        {
            yellow = value;
        }
    }
    public float Price
    {
        get
        {
            return price;
        }
        set
        {
            price = value;
        }
    }
}

public class CoinsInformation
{
    public static List<TestData> getContent()
    {
        var listContent = new List<TestData>();
        for (int i = 0; i < 4; i++)
        {
            var goodsInformation = new TestData();
            goodsInformation.Green = i + 110;
            goodsInformation.Yellow = i + 210;
            goodsInformation.Red = i + 310;
            listContent.Add(goodsInformation);
        }
        return listContent;
    }
}

public class critical_information
{
    private int times = 0;
    private Dictionary<int, int> basic_data = new Dictionary<int, int>();

    public int Times
    {
        get
        {
            return times;
        }
        set
        {
            times = value;
        }
    }

    public void times_reset()
    {
        times = 0;
    }

    public critical_information()
    {
        for (int i = 0; i < 7; i++)
        {
            basic_data.Add(i,0);
        }
    }

    public void revise(int index, int number)
    {
        basic_data[index] += number;
    }

    public int count()
    {
        return basic_data.Count;
    }

    public void reset()
    {
        for(int i = 0; i < basic_data.Count;i++)
        {
            basic_data[i] = 0;
        }
    }

    public Dictionary<int,int> getback()
    {
        return basic_data;
    }
}