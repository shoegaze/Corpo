public interface IController<in M> where M : IModel {
	void HandleInput(M model);
}