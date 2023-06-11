using Battle.Model;
using UnityEngine;

namespace Battle.View {
	public class MenuView : MonoBehaviour, IView<IMenuModel> {
		private IMenuModel model;
		
		public void Initialize(IMenuModel model) {
			this.model = model;
		}
		
		public void UpdateState(IMenuModel model) {
			throw new System.NotImplementedException();
		}

		protected void Update() {
			UpdateState(model);
		}
	}
}
