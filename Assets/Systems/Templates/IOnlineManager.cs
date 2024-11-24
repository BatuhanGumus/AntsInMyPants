
using Photon.Realtime;

public interface IOnlineManager
{
        public ClientState OnlineState { get; }
        public void Login();
}