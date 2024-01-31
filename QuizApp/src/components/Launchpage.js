import { useNavigate } from "react-router-dom";
import './Creator.css';

function Launchpage() {
  const navigate = useNavigate();

  const Home = () => {
    navigate("/quizs");
  };

  return (
    <div className="input-container-controller" >
      <h5 className="alert alert-launch">Welcome to the world of knowledge and fun! 🌟 Step into the realm of "QuizCraft", where curiosity meets excitement.</h5>
      <button className="btn btn-home" onClick={Home}>
        Launch
      </button>
    </div>
  );
}

export default Launchpage;
