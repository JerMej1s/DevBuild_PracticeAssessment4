using System;
using System.Collections.Generic;

namespace PracticeAssessment4
{
    class Student
    {
        public string Name;
        public int Status;
        public List<int> Scores;
        public char Grade;
        public Student(string _Name, int _Status, List<int> _Scores)
        {
            Name = _Name;
            Status = _Status;
            Scores = _Scores;
        }

        public char GetGrade(List<int> _Scores)
        {
            int sumScore = 0;
            for (int i = 0; i < _Scores.Count; i++) sumScore += _Scores[i];
            decimal meanScore = sumScore / _Scores.Count;
            if (meanScore >= 90) Grade = 'A';
            else if (meanScore >= 80) Grade = 'B';
            else if (meanScore >= 70) Grade = 'C';
            else if (meanScore >= 60) Grade = 'D';
            else Grade = 'E';
            return Grade;
        }        
        public static void ListAllStudents(List<Student> _AllStudents)
        {
            foreach (Student student in _AllStudents) Console.WriteLine(student);
        }
        public static Student FindOne(List<Student> _AllStudents, string _userName)
        {
            foreach (Student student in _AllStudents)
            {
                if (student.Name.StartsWith(_userName)) return student;
            }
            return null;
        }
        public override string ToString() => $"{Name}\tStatus: {Status}\tScores: {string.Join(", ", Scores)}\tGrade: {GetGrade(Scores)}";
    }
    class GradStudent : Student
    {
        List<Student> Students = new List<Student>();
        public GradStudent(string _Name, int _Status, List<int> _Scores, List<Student> _Students) : base(_Name, _Status, _Scores) => Students = _Students;
        public char GetGrade(List<Student> MyStudents)
        {
            int performanceTally = 0;
            foreach (Student student in MyStudents)
            {
                if (student.Grade == 'A') performanceTally += 10;
                else if (student.Grade == 'B') performanceTally += 9;
                else if (student.Grade == 'C') performanceTally += 8;
                else if (student.Grade == 'D') performanceTally += 7;
            }
            decimal meanPerformancePoints = performanceTally / MyStudents.Count;
            if (meanPerformancePoints < 7 || base.GetGrade(Scores) == 'D' || base.GetGrade(Scores) == 'E') Grade = 'E';
            else Grade = 'A';
            return Grade;
        }
        public static void ScoreAStudent(Student _scoreStudent, int _newScore) => _scoreStudent.Scores.Add(_newScore);
        public override string ToString() => $"{Name}\tStatus: {Status}\tScores: {string.Join(", ", Scores)}\tGrade: {GetGrade(Students)}";
    }
    class Program
    {
        static bool KeepGoing()
        {
            bool done = false;
            bool result = false;
            while (!done)
            {
                Console.Write("\nDo you want to return to the login (y/n)? ");
                string userKeepGoing = Console.ReadLine().ToLower();
                if (userKeepGoing == "y")
                {
                    result = true;
                    done = true;
                }
                else if (userKeepGoing == "n")
                {
                    result = false;
                    done = true;
                }
                else Console.WriteLine("Please enter \"y\" or \"n\"");
            }
            return result;
        }
        static void Main(string[] args)
        {
            List<Student> AllStudents = new List<Student>();

            Student student1 = new Student("Topanga Lawrence", 1, new List<int> { 80, 100 });  // Expect Grade: A
            Student student2 = new Student("Cory Matthews", 1, new List<int> { 60, 100 });  // Expect Grade: B
            Student student3 = new Student("Lisa Simpson", 1, new List<int> { 40, 100 });  // Expect Grade: C
            Student student4 = new Student("Bart Simpson", 1, new List<int> { 20, 100 });  // Expect Grade: D
            Student student5 = new Student("Ferris Bueller", 1, new List<int> { 19, 100 });  // Expect Grade: E
            Student gradStudent1 = new GradStudent("Jeremy Jones", 2, new List<int> { 90, 100 }, new List<Student> { student1, student2, student3, student4, student5 });

            AllStudents.Add(student1);
            AllStudents.Add(student2);
            AllStudents.Add(student3);
            AllStudents.Add(student4);
            AllStudents.Add(student5);
            AllStudents.Add(gradStudent1);

            Console.WriteLine("WELCOME TO GRADEBOOK");
            do
            {
                Console.Write("\nTo log in, enter your name: ");
                Student foundUser = Student.FindOne(AllStudents, Console.ReadLine());
                if (foundUser is GradStudent)
                {
                    bool quit = false;
                    do
                    {
                        Console.Write("\nDo you want to (A) see a report card or (B) grade a student? ");
                        string userChoice = Console.ReadLine();
                        switch (userChoice)
                        {
                            case "A":
                                Console.WriteLine("");
                                Student.ListAllStudents(AllStudents);
                                break;

                            case "B":
                                Console.Write("\nWhich student do you want to score? ");
                                Student scoreStudent = Student.FindOne(AllStudents, Console.ReadLine());

                                Console.Write($"\nEnter a score to assign to {scoreStudent.Name}: ");
                                int newScore = int.Parse(Console.ReadLine());

                                GradStudent.ScoreAStudent(scoreStudent, newScore);
                                Console.WriteLine($"\n{newScore} was added to {scoreStudent.Name}'s scores.");
                                break;
                        }
                        Console.Write("\nDo you want to see the grad student menu again (y/n)? ");
                        string userQuit = Console.ReadLine().ToLower();
                        if (userQuit == "n") quit = true;
                    }
                    while (!quit);
                }
                else Console.WriteLine($"\n{foundUser}");
            }
            while (KeepGoing());
        }
    }
}