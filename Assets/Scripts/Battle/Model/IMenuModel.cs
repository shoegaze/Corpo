using Battle.View;

namespace Battle.Model {
	public interface IMenuModel : IModel {
		StateManager StateManager { get; }
		// AbilitySelect AbilitySelect { get; }

		void SelectAbility();
		void CycleAbility(int dy);
		void LoadAbility();
	}
}
