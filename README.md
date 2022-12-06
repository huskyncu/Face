# IOT 12/05 課後作業 Face


## DEMO影片
[DEMO影片](https://www.youtube.com/watch?v=OqhyZvl7nwE)

## 運作流程
大致流程如影片所示：

### preview 
* 下方的欄位貼上網址去辨識該圖片的人臉的年紀跟性別。

* 住控台後台輸出like: 
```
    DecteFace response : {"faceId":"f33106e1-6909-4119-bc68-e65a77f8c332","faceRectangle":{"top":169,"left":447,"width":180,"height":180},"faceAttributes":{"gender":"female","age":22.0}}
DecteFace resultStr : female,22
```

* 接著就會顯示圖片、年紀、性別！
![](https://i.imgur.com/E17uzS4.png)


### 訓練
* 接下來我們要create person，create person我們這次有兩個訊練群組，一個是習近平的圖片，一個是陳紫渝的圖片。


* 紫渝部分
![](https://i.imgur.com/T1wDd2Q.png)

    後台會編辨識邊加入。
    ```
    jsonData : { name = 紫渝 }
    CreatePerson response : {"personId":"9396ece5-976a-4615-8a34-0c1ab7b564ab"}
    ListAllPersons response : [{"personId":"9396ece5-976a-4615-8a34-0c1ab7b564ab","persistedFaceIds":[],"name":"紫渝"}]
    DecteFace response : {"faceId":"2d21ba98-136d-46a7-a09c-a16e549090b7","faceRectangle":{"top":179,"left":448,"width":81,"height":81},"faceAttributes":{"gender":"female","age":22.0}}
    DecteFace resultStr : 448,179,81,81
    AddFace response : {"persistedFaceId":"2752426c-5a05-40c3-a7bd-b68ddeb3a5c5"}
    Train response : 
    AddFace isSuccessed : True
    DecteFace response : {"faceId":"c7e52672-32d5-4d1b-b9ad-d298db70dd4f","faceRectangle":{"top":105,"left":284,"width":168,"height":168},"faceAttributes":{"gender":"female","age":22.0}}
    DecteFace resultStr : 284,105,168,168
    AddFace response : {"persistedFaceId":"c3d3188e-718d-43a6-9408-cb51c0b9ca7c"}
    Train response : 
    AddFace isSuccessed : True
    DecteFace response : {"faceId":"b97bfd55-e659-4cbf-a6d0-fc55b11c16e9","faceRectangle":{"top":142,"left":288,"width":53,"height":53},"faceAttributes":{"gender":"female","age":25.0}}
    DecteFace resultStr : 288,142,53,53
    AddFace response : {"persistedFaceId":"56a43ddd-1a4f-4f8d-929e-0040224f8328"}
    Train response : 
    AddFace isSuccessed : True
    DecteFace response : {"faceId":"40407876-0d17-43bb-b9c1-ef2ad8e06465","faceRectangle":{"top":66,"left":288,"width":61,"height":61},"faceAttributes":{"gender":"female","age":22.0}}
    DecteFace resultStr : 288,66,61,61
    AddFace response : {"persistedFaceId":"fc2e6ab0-2840-4236-8f30-d70c2229c97e"}
    Train response : 
    AddFace isSuccessed : True
    DecteFace response : {"faceId":"720d421f-ebad-4db1-a833-d326e59cc87b","faceRectangle":{"top":101,"left":381,"width":120,"height":120},"faceAttributes":{"gender":"female","age":22.0}}
    DecteFace resultStr : 381,101,120,120
    AddFace response : {"persistedFaceId":"36a020fe-76aa-4e8a-ba96-a37790263550"}
    Train response : 
    AddFace isSuccessed : True
    ```

    總之就是這些就是確認我們訓練的face資料可以被正確加入。


* 然後習大大的會在做一次。
    ```
    jsonData : { name = 習大大 }
    CreatePerson response : {"personId":"efde888f-d7ea-4c87-803d-b9139fa13f30"}
    ListAllPersons response : [{"personId":"9396ece5-976a-4615-8a34-0c1ab7b564ab","persistedFaceIds":["2752426c-5a05-40c3-a7bd-b68ddeb3a5c5","36a020fe-76aa-4e8a-ba96-a37790263550","56a43ddd-1a4f-4f8d-929e-0040224f8328","c3d3188e-718d-43a6-9408-cb51c0b9ca7c","fc2e6ab0-2840-4236-8f30-d70c2229c97e"],"name":"紫渝"},{"personId":"efde888f-d7ea-4c87-803d-b9139fa13f30","persistedFaceIds":[],"name":"習大大"}]
    DecteFace response : {"faceId":"531309bd-e9f9-48db-8b0e-734e4efd3d02","faceRectangle":{"top":118,"left":140,"width":93,"height":93},"faceAttributes":{"gender":"male","age":57.0}}
    DecteFace resultStr : 140,118,93,93
    AddFace response : {"persistedFaceId":"762b7f70-f88c-43ab-88ef-5a6e16f4730a"}
    Train response : 
    AddFace isSuccessed : True
    DecteFace response : {"faceId":"11ab1d65-4abf-44e4-a471-573a85b6ccce","faceRectangle":{"top":170,"left":200,"width":133,"height":133},"faceAttributes":{"gender":"male","age":57.0}}
    DecteFace resultStr : 200,170,133,133
    AddFace response : {"persistedFaceId":"f1ea4408-057f-488a-a81b-8cafd66ffcfa"}
    Train response : 
    AddFace isSuccessed : True
    DecteFace response : {"faceId":"3e1af217-b173-4b8b-aa18-ece32329df3c","faceRectangle":{"top":429,"left":419,"width":657,"height":657},"faceAttributes":{"gender":"male","age":49.0}}
    DecteFace resultStr : 419,429,657,657
    AddFace response : {"persistedFaceId":"f43ca084-8723-4152-a65f-4a9735db882c"}
    Train response : 
    AddFace isSuccessed : True
    DecteFace response : {"faceId":"d796cad4-d48c-4cec-a246-c39d7d60b06e","faceRectangle":{"top":227,"left":421,"width":262,"height":262},"faceAttributes":{"gender":"male","age":56.0}}
    DecteFace resultStr : 421,227,262,262
    AddFace response : {"persistedFaceId":"a8cc772a-afad-42c9-a4d3-1bb900faf0c9"}
    Train response : 
    AddFace isSuccessed : True
    DecteFace response : {"faceId":"8ddab1e1-1f2d-4ea8-b414-dbf6632e7608","faceRectangle":{"top":202,"left":182,"width":289,"height":289},"faceAttributes":{"gender":"male","age":55.0}}
    DecteFace resultStr : 182,202,289,289
    AddFace response : {"persistedFaceId":"eab94c9c-6cd5-46f8-99fb-ef601f81dcdf"}
    Train response : 
    AddFace isSuccessed : True
    ```
* 訓練完成
每次訓練完成就會有提示通知訓練完成。
![](https://i.imgur.com/FaFIO8h.png)

### 辨識
訓練完成就會有兩個source了，接下來進行辨識。就會發現他辨識出照片是紫渝，信心值為92。
```
DecteFace response : {"faceId":"ffe0897b-b7d9-487b-abd1-c88d252a50b0","faceRectangle":{"top":169,"left":447,"width":180,"height":180},"faceAttributes":{"gender":"female","age":22.0}}
DecteFace resultStr : female,22
DecteFace response : {"faceId":"cec6d554-ba91-4d57-856d-1915eb40b809","faceRectangle":{"top":169,"left":447,"width":180,"height":180},"faceAttributes":{"gender":"female","age":22.0}}
DecteFace resultStr : cec6d554-ba91-4d57-856d-1915eb40b809
IdentifyFace response : {"faceId":"cec6d554-ba91-4d57-856d-1915eb40b809","candidates":[{"personId":"9396ece5-976a-4615-8a34-0c1ab7b564ab","confidence":0.91919}]}
GetPersonByID response : {"personId":"9396ece5-976a-4615-8a34-0c1ab7b564ab","persistedFaceIds":["2752426c-5a05-40c3-a7bd-b68ddeb3a5c5","36a020fe-76aa-4e8a-ba96-a37790263550","56a43ddd-1a4f-4f8d-929e-0040224f8328","c3d3188e-718d-43a6-9408-cb51c0b9ca7c","fc2e6ab0-2840-4236-8f30-d70c2229c97e"],"name":"紫渝"}
DecteFace response : {"faceId":"19a2a621-0bc4-42a4-b827-b2a1c138c8cd","faceRectangle":{"top":169,"left":447,"width":180,"height":180},"faceAttributes":{"gender":"female","age":22.0}}
DecteFace resultStr : female,22
```
![](https://i.imgur.com/KKvEOQc.png)


### 看訓練圖片
* 點擊訓練圖片就會得到下方所有訓練圖片。
![](https://i.imgur.com/oi05P3g.png)

    ```
    ListAllPersons response : [{"personId":"9396ece5-976a-4615-8a34-0c1ab7b564ab","persistedFaceIds":["2752426c-5a05-40c3-a7bd-b68ddeb3a5c5","36a020fe-76aa-4e8a-ba96-a37790263550","56a43ddd-1a4f-4f8d-929e-0040224f8328","c3d3188e-718d-43a6-9408-cb51c0b9ca7c","fc2e6ab0-2840-4236-8f30-d70c2229c97e"],"name":"紫渝"},{"personId":"efde888f-d7ea-4c87-803d-b9139fa13f30","persistedFaceIds":["762b7f70-f88c-43ab-88ef-5a6e16f4730a","a8cc772a-afad-42c9-a4d3-1bb900faf0c9","eab94c9c-6cd5-46f8-99fb-ef601f81dcdf","f1ea4408-057f-488a-a81b-8cafd66ffcfa","f43ca084-8723-4152-a65f-4a9735db882c"],"name":"習大大"}]
    GetFaceByID response : {"persistedFaceId":"2752426c-5a05-40c3-a7bd-b68ddeb3a5c5","userData":"https://www.upmedia.mg/upload/article/20221202121806181178.png"}
    GetFaceByID response : {"persistedFaceId":"36a020fe-76aa-4e8a-ba96-a37790263550","userData":"https://media.zenfs.com/ko/setn.com.tw/41a429564c4fd7bcae7b5ac831f44176"}
    GetFaceByID response : {"persistedFaceId":"56a43ddd-1a4f-4f8d-929e-0040224f8328","userData":"https://img-s-msn-com.akamaized.net/tenant/amp/entityid/AA142YY8.img?h=315&w=600&m=6&q=60&o=t&l=f&f=jpg&x=357&y=192"}
    GetFaceByID response : {"persistedFaceId":"c3d3188e-718d-43a6-9408-cb51c0b9ca7c","userData":"https://s.newtalk.tw/album/news/802/62f891b1537e6.jpg"}
    GetFaceByID response : {"persistedFaceId":"fc2e6ab0-2840-4236-8f30-d70c2229c97e","userData":"https://4gtvimg.4gtv.tv/4gtv-Image/Production/Article/2022092605000027/202209260602038527.jpg"}
    GetFaceByID response : {"persistedFaceId":"762b7f70-f88c-43ab-88ef-5a6e16f4730a","userData":"https://th.bing.com/th/id/OIP.F9bAjDjRZ2eVhGybJRKlowHaE5?pid=ImgDet&rs=1"}
    GetFaceByID response : {"persistedFaceId":"a8cc772a-afad-42c9-a4d3-1bb900faf0c9","userData":"https://www.iza.ne.jp/kiji/world/images/201120/wor20112023080022-m1.jpg"}
    GetFaceByID response : {"persistedFaceId":"eab94c9c-6cd5-46f8-99fb-ef601f81dcdf","userData":"https://www.1242.com/lf/asset/uploads/2017/08/Xi_Jinping_2016a.jpg"}
    GetFaceByID response : {"persistedFaceId":"f1ea4408-057f-488a-a81b-8cafd66ffcfa","userData":"https://news.1242.com/wp-content/uploads/2017/10/jpp025321783a-1.jpg"}
    GetFaceByID response : {"persistedFaceId":"f43ca084-8723-4152-a65f-4a9735db882c","userData":"https://zjnews.zjol.com.cn/ztjj/2017sjdzt/sjdqmtbd/201710/W020171025690874672386.jpg"}
    ```

<!--
### 運作邏輯
我們會有一個訓練的training source，他負責蒐集資料。

用一個API ACtion去存取他們的偵測、辨識、創建persongroup的function。

用一個json obj (名字是PersonObj)去存取 圖片的各個資訊 ID, name, gender, age, 信心值。(依據不同狀況會有不同存取組合)
 


以上就是我們程式的運作流程。

<!--
### 分工

<!---->

 
