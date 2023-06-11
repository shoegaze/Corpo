public interface IView<in M> where M : IModel {
	void Initialize(M model);
	void UpdateState(M model);
}