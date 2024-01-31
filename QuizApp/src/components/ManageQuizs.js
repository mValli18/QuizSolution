import { useNavigate } from "react-router-dom";
function ManageQuizs(){
    const navigate=useNavigate();
    <div className="inputcontroller">
        <button className="btn btn-primary" onClick={handleGet}>Get Quizs</button>
        <button className="btn btn-primary" onClick={handleAdd}>Add Quiz</button>
        <button className="btn btn-primary" onClick={handleUpdate}>Update Quiz</button>
        <button className="btn btn-primary" onClick={handleDelete}>Delete Quiz</button>
    </div>
}
export default ManageQuizs;