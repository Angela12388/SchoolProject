
function FixedUpdate(){

if(!GetComponent.<Renderer>().material.mainTexture.IsPlaying){

GetComponent.<Renderer>().material.mainTexture.Play();

}
}