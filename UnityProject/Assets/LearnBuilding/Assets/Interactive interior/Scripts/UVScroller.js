var Speed = 0.5; 

function Update () 

{

var offset = Time.time * Speed;

GetComponent.<Renderer>().material.SetTextureOffset ("_MainTex", Vector2(0,offset));
GetComponent.<Renderer>().material.SetTextureOffset ("_BumpMap", Vector2(0,offset));

}


