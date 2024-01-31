import { useState ,useEffect} from "react";
import { useNavigate } from "react-router-dom";

function AddQuiz() {
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [category, setCategory] = useState("");
  const [timeLimit, setTimeLimit] = useState("0"); 
  const token=localStorage.getItem("token");
  const [categoryList, setCategoryList] = useState([]);
  const [categoryInput, setCategoryInput] = useState("");
  const [newCategoryInput,setNewCategoryInput]=useState("");
  const [isAddingNewCategory, setIsAddingNewCategory] = useState(false);
  const navigate=useNavigate();
  const clickAdd = () => {
    // Check if required fields are provided
    if (!title || !description || (isAddingNewCategory&&!newCategoryInput)) {
      alert("Title, Description, and Category are required fields.");
      return;
    }
    const selectedCategory = isAddingNewCategory ? newCategoryInput : category;
    const quiz = {
      title: title,
      description: description,
      category: selectedCategory,
      timeLimit: timeLimit || null, // Set to null if timeLimit is empty
    };

    console.log(quiz);

    fetch("http://localhost:5252/api/Quiz", {
      method: "POST",
      headers: {
        Accept: "application/json",
        Authorization: `Bearer ${token}`,
        "Content-Type": "application/json",
      },
      body: JSON.stringify(quiz),
    })
      .then(async(response) => {
        if(!response.ok){
          throw new Error(`Error: ${response.statusText}`);
        }
        alert("Quiz Added successfully");
        navigate("/quizList");
      })
      .catch((e) => {
          alert("Please provide all values and timelimit should be integer");
        console.log(e);
      });
  };

  useEffect(() => {
    // Fetch the list of categories when the component mounts
    fetch("http://localhost:5252/api/Quiz/categories", {
      method: 'GET',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      }
    })
      .then(async (data) => {
        var categories = await data.json();
        setCategoryList(categories);
      })
      .catch((e) => {
        console.log(e);
      });
  }, []);
  const handleCategoryChange = (e) => {
    const selectedCategory = e.target.value;

    if (selectedCategory === "new") {
      setIsAddingNewCategory(true);
      setCategoryInput(""); // Reset categoryInput
    } else {
      setIsAddingNewCategory(false);
      setCategoryInput(selectedCategory);
    }
  };
  const handleNewCategoryInput=(e)=>{
    setNewCategoryInput(e.target.value);
  };
  return (
    <div className="inputcontainer">
      <h1 className="alert alert-quiz">Quiz Details</h1>
      <div class="form-floating mb-3">
      <input
        id="floatingInput"
        type="text"
        className="form-control"
        placeholder="Quiz Title"
        value={title}
        onChange={(e) => {
          setTitle(e.target.value);
        }}
      />
      <label htmlFor="floatingInput">
        Quiz Title
      </label>
      </div>
      <div class="form-floating mb-3">
      <input
        id="floatingInput"
        type="text"
        className="form-control"
        placeholder="Quiz Description"
        value={description}
        onChange={(e) => {
          setDescription(e.target.value);
        }}
      />
      <label htmlFor="floatingInput">
        Quiz Description
      </label>
      </div>
      
      <div class="form-floating mb-3">

      <select className="form-select" value={categoryInput} placeholder="Category" onChange={handleCategoryChange}>
          <option value="">Select a category</option>
          {categoryList.map((category) => (
            <option key={category} value={category}>
              {category}
            </option>
          ))}
          <option value="new">Add New Category</option>
        </select>
        {isAddingNewCategory && (
        <div className="form-floating mb-3">
          <input
            id="newCategoryInput"
            type="text"
            className="form-control"
            placeholder="New Category"
            value={newCategoryInput}
            onChange={handleNewCategoryInput}
          />
          <label htmlFor="newCategoryInput">New Category</label>
        </div>
      )}
      <div class="form-floating mb-3">
      <input
        id="floatingInput"
        type="number"
        className="form-control"
        value={timeLimit}
        placeholder="Please provide integer value in minutes."
        onChange={(e) => {
          setTimeLimit(e.target.value);
        }}
      />
      <label htmlFor="floatingInput">
        Integer timeLimit in Minutes
      </label>
      </div>
      <button onClick={clickAdd} className="btn btn-primary">
        Add Quiz
      </button>
      </div>
    </div>
  );
}

export default AddQuiz;