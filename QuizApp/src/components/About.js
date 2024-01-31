import React from 'react';

function About(){
  return (
    <div className='alert alert-quiz'>
      <div class="icon">
                <img src="./icon.jpg" alt="QuizCraft"/>
            </div>
      <h2>QuizCraft - About</h2>
      <h5>Purpose of QuizCraft</h5>
      <p>
        The QuizCraft app is a web-based application designed to offer an engaging and educational experience for quiz takers, while providing quiz creators with a platform to share their knowledge and quizzes.
      </p>
      <h5>Features</h5>
        <li>
          <strong>User Roles:</strong>
          <ul>
            <li>Two user roles: Quiz Taker and Quiz Creator.</li>
            <li>Quiz Takers can browse and participate in quizzes.</li>
            <li>Quiz Creators can create, edit, and delete quizzes.</li>
          </ul>
        </li>

        <li>
          <strong>Quiz Creation:</strong>
          <ul>
            <li>Quiz Creators can create new quizzes with a title and description.</li>
            <li>Add multiple-choice questions with options and correct answers.</li>
            <li>Specify the time limit for each quiz if they want to.</li>
          </ul>
        </li>
        <li>
            <strong>Quiz Taking</strong>
            <ul>
                <li>Quiz Takers can browse and select from available quizzes.</li>
                <li>There will be timer for timelimited quizzes.</li>
                <li>Users can select answers for multiple-choice questions.</li>
                <li>Once a quiz is attempted by user that quiz can't be answered again.</li>
            </ul>
        </li>
        <li>
            <strong>Quiz Results</strong>
            <ul>
            <li>The score of the quizzes will be displayed automatically upon quiz completion.</li>
            <li>The Status of user answer is also displayed as answer is correct/incorrect.</li>
            <li>If the user tries to open the quiz that is already answered the respective quiz results will be displayed.</li>
            </ul>
        </li>
        <li>
            <strong>Leaderboard</strong>
            <ul>
            <li>The Leaderboard can be accessed by selecting the required quiz title from the dropdown menu</li>
            <li>The top scorer will be displayed on top and vice versa</li>
            </ul>
        </li>
      <h5>Targeted Users</h5>
      <p>This QuizCraft is intended to be used by the students and any trivia enthusiasts</p>
      <h5>Developer Information</h5>
      <p>The QuizCraft is developed and maintained by <strong>Srimayee</strong> and <strong>Srivalli</strong> we made our QuizCraft Application reliable. To share the feedback of our QuizCraft Application please contact us at quizcraft@yahoo.com</p>
      <h5>Privacy Policy</h5>
      <p>We hereby clearly declare that no personal data of the user is asked and maintained in our QuizCraft applications.
         The quizresults of the participants are acceccible by the quiz creators and it is cautioned to not share the QuizCraft credentials with anyone to avoid cheating.
      </p>
    </div>
  );
};

export default About;