using EntityStates;
using R2API.Networking.Interfaces;
using RoR2;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.Messages {

    internal struct StateMessage(EntityStateMachine stateMachine, EntityState state) : INetMessage {
        private EntityStateMachine _stateMachine = stateMachine;
        private EntityState _state = state;

        public void Deserialize(NetworkReader reader) {
            var networkIdentity = reader.ReadNetworkIdentity();
            var stateMachineIndex = reader.ReadByte();
            var entityStateIndex = reader.ReadEntityStateIndex();
            if (networkIdentity &&
                networkIdentity.hasAuthority &&
                networkIdentity.gameObject.TryGetComponent<NetworkStateMachine>(out var networkStateMachine)) {
                _stateMachine = networkStateMachine.stateMachines.ElementAtOrDefault(stateMachineIndex);
                if ((_state = EntityStateCatalog.InstantiateState(entityStateIndex)) != null) {
                    _state.outer = _stateMachine;
                }
            }
        }

        public readonly void OnReceived() {
            if (_stateMachine) {
                _stateMachine.SetNextState(_state);
            }
        }

        public readonly void Serialize(NetworkWriter writer) {
            writer.Write(_stateMachine.networkIdentity);
            writer.Write((byte)_stateMachine.networkIndex);
            writer.Write(EntityStateCatalog.GetStateIndex(_state.GetType()));
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Register() {
            if (R2API.Networking.NetworkingAPI.RegisterMessageType<StateMessage>()) {
                Main.Logger.LogMessage("StateMessage Register Successd!");
            } else {
                Main.Logger.LogError("StateMessage Register Failed!");
            }
        }
    }
}