using Photon.Pun;

public class MapLoader : MonoBehaviourPunCallbacks
{
    private readonly string _levelName = "Game";
        
    public override void OnJoinedRoom()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 60;
        LoadScene();
    }

    private void LoadScene()
    {
        PhotonNetwork.LoadLevel(_levelName);
    }
}
