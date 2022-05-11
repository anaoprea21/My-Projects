using HangMan.View;
using HangMan.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml;


namespace HangMan.Services
{
    class UserHandeler : INotifyPropertyChanged
    {
        private ObservableCollection<User> p_users;
        public ObservableCollection<User> users
        {
            get
            {
                return p_users;
            }
            set
            {
                p_users = value;
                OnPropertyChanged("users");
            }
        }
        public UserHandeler()
        {
            users = new ObservableCollection<User>();
            LoadUsers();

            //----------game handler constructor----------
            LoadImages();
            currentHangman = hangman[0];
            category = "None";
            blankWord = "Choose a category";
            wordGuess = "";
            score = 0;
            lives = 5;
            startWasPressed = false;
            aWasPressed = false;
            bWasPressed = false;
            cWasPressed = false;
            dWasPressed = false;
            eWasPressed = false;
            fWasPressed = false;
            gWasPressed = false;
            hWasPressed = false;
            iWasPressed = false;
            jWasPressed = false;
            kWasPressed = false;
            lWasPressed = false;
            mWasPressed = false;
            nWasPressed = false;
            oWasPressed = false;
            pWasPressed = false;
            rWasPressed = false;
            sWasPressed = false;
            tWasPressed = false;
            uWasPressed = false;
            vWasPressed = false;
            wWasPressed = false;
            xWasPressed = false;
            yWasPressed = false;
            zWasPressed = false;
        }

        private int p_currentSelected;
        public int currentSelected
        {
            get { return p_currentSelected; }
            set
            {
                p_currentSelected = value;
                OnPropertyChanged("currentSelected");
            }
        }

        //property changed
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            VerifyPropertyName(propertyName);
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void VerifyPropertyName(string propertyName)
        {
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
                throw new ArgumentNullException(GetType().Name + " does not contain property: " + propertyName);
        }

        //-----------------------------------user functions-----------------------------------
        public void AddUser(object obj)
        {
            NewUser window = new NewUser();
            window.ShowDialog();
            string name = "", pathIm = "";


            name = window.username.Text;
            pathIm = window.path.Text;

            User newUser = new User(name, pathIm);

            XmlDocument doc = new XmlDocument();
            doc.Load("C:\\Users\\anaop\\source\\repos\\HangMan\\HangMan\\Model\\users.xml");

            XmlElement user = doc.CreateElement("user");
            user.SetAttribute("username", name);
            user.SetAttribute("img", pathIm);

            doc.DocumentElement.AppendChild(user);
            doc.Save("C:\\Users\\anaop\\source\\repos\\HangMan\\HangMan\\Model\\users.xml");

            users.Add(newUser);
            OnPropertyChanged("users");
        }
        public void LoadUsers()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("C:\\Users\\anaop\\source\\repos\\HangMan\\HangMan\\Model\\users.xml");
            foreach (XmlNode nod in doc.DocumentElement.ChildNodes)
            {
                string name = nod.Attributes["username"]?.InnerText;
                string image = nod.Attributes["img"]?.InnerText;
                User u = new User(name, image);
                users.Add(u);
                OnPropertyChanged("users");
            }
        }
        public void DeleteUser(object obj)
        {
            users.Remove(users[currentSelected]);

            XmlDocument doc = new XmlDocument();
            doc.Load("C:\\Users\\anaop\\source\\repos\\HangMan\\HangMan\\Model\\users.xml");
            doc.DocumentElement.RemoveAll();
            for (int i = 0; i < users.Count; i++)
            {
                User u = users[i];
                string name = u.username;
                string image = u.imagePath;
                XmlElement user = doc.CreateElement("user");
                user.SetAttribute("username", name);
                user.SetAttribute("img", image);
                doc.DocumentElement.AppendChild(user);
            }
            doc.Save("C:\\Users\\anaop\\source\\repos\\HangMan\\HangMan\\Model\\users.xml");
            OnPropertyChanged("users");
        }

        ICommand addUser;
        ICommand deleteUser;

        public ICommand addNewUser
        {
            get
            {
                if (addUser == null)
                    addUser = new RelayCommand(AddUser);
                return addUser;
            }
        }
        public ICommand deleteCurentUser
        {
            get
            {
                if (deleteUser == null)
                    deleteUser = new RelayCommand(DeleteUser);
                return deleteUser;
            }
        }

        //-------------------------------------game handler-----------------------------------

        Player player;

        public int score;

        private int p_lives;
        public int lives
        {
            get { return p_lives; }
            set
            {
                p_lives = value;
                OnPropertyChanged("lives");
            }
        }

        public string[] words;

        private string p_blankWord;
        public string blankWord
        {
            get { return p_blankWord; }
            set
            {
                p_blankWord = value;
                OnPropertyChanged("blankWord");
            }
        }

        private string p_wordGuess;
        public string wordGuess
        {
            get { return p_wordGuess; }
            set
            {
                p_wordGuess = value;
                OnPropertyChanged("wordGuess");
            }
        }

        public string[] hangman;

        public int hangmanIndex = 0;

        private string p_currentHangman;
        public string currentHangman
        {
            get
            {
                return p_currentHangman;
            }
            set
            {
                p_currentHangman = value;
                OnPropertyChanged("currentHangman");

            }
        }

        private string p_category;
        public string category
        {
            get { return p_category; }
            set
            {
                p_category = value;
                OnPropertyChanged("category");
            }
        }

        //------------------------------------------game functions---------------------------------
       
        ICommand m_gameStart;
        public ICommand GameStartCommand
        {
            get
            {
                if (m_gameStart == null)
                    m_gameStart = new RelayCommand(GameStart);
                return m_gameStart;
            }
        }
        private void GameStart(object obj)
        {
            player = new Player();
            NewGame game = new NewGame(users[currentSelected].username);
            player.username = users[currentSelected].username;
            string path = "C:\\Users\\anaop\\source\\repos\\HangMan\\HangMan\\Model\\allStatistics.txt";
            string[] textFile = File.ReadAllLines(path);
            for (int i = 0; i < textFile.Length; i += 3)
            {
                string aux = textFile[i];
                if (aux.Equals(player.username))
                {
                    player.games = Int32.Parse(textFile[i + 1]);
                    player.gamesWon = Int32.Parse(textFile[i + 2]);
                    break;
                }
            }

            //il ia din all si il pune in current
            path = "C:\\Users\\anaop\\source\\repos\\HangMan\\HangMan\\Model\\playerData.txt";
            using (StreamWriter outputFile = new StreamWriter(path))
            {
                outputFile.WriteLine(player.username);
                outputFile.WriteLine(player.games);
                outputFile.WriteLine(player.gamesWon);
            }
            game.Show();
        }

        //-------------------open game---------------

        private ICommand m_loadGame;
        public ICommand LoadGameCommand
        {
            get
            {
                if (m_loadGame == null)
                    m_loadGame = new RelayCommand(LoadGame);
                return m_loadGame;
            }
        }
        private void LoadGame(object obj)
        {
            OpenFileDialog open = new OpenFileDialog();
            words = new string[100];
            open.Filter = "Text file|*.txt";
            open.DefaultExt = ".txt";
            string path = "C:\\Users\\anaop\\source\\repos\\HangMan\\HangMan\\Model\\playerData.txt";
            string[] textFile = File.ReadAllLines(path);
            player = new Player(textFile[0], Int32.Parse(textFile[1]), Int32.Parse(textFile[2]));
            if (open.ShowDialog() == true)
            {
                string[] content = File.ReadAllLines(open.FileName);
                if (player.username == content[0])
                {
                    wordGuess = content[1];
                    blankWord = content[2];
                    lives = Int32.Parse(content[3]);
                    hangmanIndex = 5 - lives;
                    currentHangman = hangman[hangmanIndex];
                    score = Int32.Parse(content[4]);
                    for (int i = 5; i < content.Length; i++)
                    {
                        words[i - 5] = content[i];
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("This is not your save file.Please try again", "Error", MessageBoxButton.OK);
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Eror 403: You need to open a save file to play", "Error", MessageBoxButton.OK);
            }
        }

        //-------------------reset game---------------

        private ICommand m_resetGame;
        public ICommand ResetGameCommand
        {
            get
            {
                if (m_resetGame == null)
                    m_resetGame = new RelayCommand(ResetGame);
                return m_resetGame;
            }
        }
        private void ResetGame(object obj)
        {
            currentHangman = hangman[0];
            category = "None";
            blankWord = "Choose a category";
            wordGuess = "";
            lives = 5;
            startWasPressed = false;
            aWasPressed = false;
            bWasPressed = false;
            cWasPressed = false;
            dWasPressed = false;
            eWasPressed = false;
            fWasPressed = false;
            gWasPressed = false;
            hWasPressed = false;
            iWasPressed = false;
            jWasPressed = false;
            kWasPressed = false;
            lWasPressed = false;
            mWasPressed = false;
            nWasPressed = false;
            oWasPressed = false;
            pWasPressed = false;
            rWasPressed = false;
            sWasPressed = false;
            tWasPressed = false;
            uWasPressed = false;
            vWasPressed = false;
            wWasPressed = false;
            xWasPressed = false;
            yWasPressed = false;
            zWasPressed = false;
        }

        //-------------------save game----------------

        ICommand m_saveGame;
        public ICommand SaveGameCommand
        {
            get
            {
                if (m_saveGame == null)
                    m_saveGame = new RelayCommand(SaveGame);
                return m_saveGame;
            }
        }
        private void SaveGame(object obj)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Text file|*.txt";
            string[] dataToSave = new string[100];
            string path = "C:\\Users\\anaop\\source\\repos\\HangMan\\HangMan\\Model\\playerData.txt";
            string[] textFile = File.ReadAllLines(path);
            player = new Player(textFile[0], Int32.Parse(textFile[1]), Int32.Parse(textFile[2]));
            dataToSave[0] = player.username;
            dataToSave[1] = wordGuess;
            dataToSave[2] = blankWord;
            dataToSave[3] = lives.ToString();
            dataToSave[4] = score.ToString();

            for (int i = 0; i < words.Length; i++)
            {
                dataToSave[i + 5] = words[i];
            }
            string content = "";
            for (int i = 0; i < dataToSave.Length; i++)
            {
                content += dataToSave[i];
                content += "\n";
            }
            Console.WriteLine(content);
            if (saveFile.ShowDialog() == true)
            {
                System.IO.File.WriteAllText(saveFile.FileName, content);
            }

        }

        //------------------choose word---------------
        private void LoadWord(string path)
        {
            words = File.ReadAllLines(path);
            Random random = new Random();
            int randWordIndex = random.Next(words.Length);
            wordGuess = words[randWordIndex];
            words = words.Where((source, index) => index != randWordIndex).ToArray();
            blankWord = MakeGuessWord(wordGuess);
        }
        private string MakeGuessWord(string wordGuess)
        {
            blankWord = "";
            for (int i = 0; i < wordGuess.Length; i++)
            {
                if (wordGuess[i].ToString() != " ")
                {
                    blankWord += "*";
                }
                else
                {
                    blankWord += " ";
                }
            }
            return blankWord;
        }
        private void GetNextGuessWord()
        {
            Random random = new Random();
            int randWordIndex = random.Next(words.Length);
            wordGuess = words[randWordIndex];
            words = words.Where((source, index) => index != randWordIndex).ToArray();
            blankWord = MakeGuessWord(wordGuess);
        }

        //---------------------load-------------------
        public void LoadImages()
        {
            hangman = new string[6];
            hangman[0] = "C:\\Users\\anaop\\source\\repos\\HangMan\\HangMan\\HangManImages\\hngm1.png";
            hangman[1] = "C:\\Users\\anaop\\source\\repos\\HangMan\\HangMan\\HangManImages\\hngm2.png";
            hangman[2] = "C:\\Users\\anaop\\source\\repos\\HangMan\\HangMan\\HangManImages\\hngm3.png";
            hangman[3] = "C:\\Users\\anaop\\source\\repos\\HangMan\\HangMan\\HangManImages\\hngm5.png";
            hangman[4] = "C:\\Users\\anaop\\source\\repos\\HangMan\\HangMan\\HangManImages\\hngm6.png";
            hangman[5] = "C:\\Users\\anaop\\source\\repos\\HangMan\\HangMan\\HangManImages\\hngm7.png";
        }

        ICommand loadFlowers;
        public ICommand LoadFlowersCommand
        {
            get
            {
                if (loadFlowers == null)
                    loadFlowers = new RelayCommand(LoadFlowers);
                return loadFlowers;

            }
        }
        public void LoadFlowers(object obj)
        {
            LoadWord("C:\\Users\\anaop\\source\\repos\\HangMan\\HangMan\\Words\\flowers.txt");
            category = "flowers";
        }

        ICommand loadFood;
        public ICommand LoadFoodCommand
        {
            get
            {
                if (loadFood == null)
                    loadFood = new RelayCommand(LoadFood);
                return loadFood;

            }
        }
        public void LoadFood(object obj)
        {
            LoadWord("C:\\Users\\anaop\\source\\repos\\HangMan\\HangMan\\Words\\food.txt");
            category = "food";
        }

        ICommand loadHoliday;
        public ICommand LoadHolidayCommand
        {
            get
            {
                if (loadHoliday == null)
                    loadHoliday = new RelayCommand(LoadHoliday);
                return loadHoliday;

            }
        }
        public void LoadHoliday(object obj)
        {
            LoadWord("C:\\Users\\anaop\\source\\repos\\HangMan\\HangMan\\Words\\holiday.txt");
            category = "holiday";
        }

        ICommand loadRivers;
        public ICommand LoadRiversCommand
        {
            get
            {
                if (loadRivers == null)
                    loadRivers = new RelayCommand(LoadRivers);
                return loadRivers;

            }
        }
        public void LoadRivers(object obj)
        {
            LoadWord("C:\\Users\\anaop\\source\\repos\\HangMan\\HangMan\\Words\\rivers.txt");
            category = "rivers";
        }

        ICommand loadAll;
        public ICommand LoadAllCommand
        {
            get
            {
                if (loadAll == null)
                    loadAll = new RelayCommand(LoadAll);
                return loadAll;

            }
        }
        public void LoadAll(object obj)
        {
            Random random = new Random();
            int file = random.Next(4);
            if (file == 0)
            {
                LoadWord("C:\\Users\\anaop\\source\\repos\\HangMan\\HangMan\\Words\\flowers.txt");
                category = "flowers";
            }
            else if (file == 1)
            {
                LoadWord("C:\\Users\\anaop\\source\\repos\\HangMan\\HangMan\\Words\\food.txt");
                category = "food";
            }
            else if (file == 2)
            {
                LoadWord("C:\\Users\\anaop\\source\\repos\\HangMan\\HangMan\\Words\\holiday.txt");
                category = "holiday";
            }
            else if (file == 3)
            {
                LoadWord("C:\\Users\\anaop\\source\\repos\\HangMan\\HangMan\\Words\\rivers.txt");
                category = "rivers";
            }
        }

        //-----------------statistics---------------
        private void SaveStatistics(Player player)
        {
            string path = "C:\\Users\\anaop\\source\\repos\\HangMan\\HangMan\\Model\\allStatistics.txt";
            string[] anotherTextFile = File.ReadAllLines(path);
            this.player = player;
            using (StreamWriter outputFile = new StreamWriter(path))
            {
                int counter = 4;
                foreach (string line in anotherTextFile)
                {
                    if (line == this.player.username)
                        counter = 3;
                    if (counter == 3)
                    {
                        outputFile.WriteLine(line);
                        counter -= 1;
                    }
                    else if (counter == 2)
                    {
                        outputFile.WriteLine(this.player.games);
                        counter -= 1;
                    }
                    else if (counter == 1)
                    {
                        outputFile.WriteLine(this.player.gamesWon);
                        counter -= 1;
                    }
                    else
                    {
                        Console.WriteLine(line);
                        outputFile.WriteLine(line);
                    }
                }

            }
        }

        ICommand showStat;
        public ICommand showStatCommand
        {
            get
            {
                if (showStat == null)
                    showStat = new RelayCommand(ShowStatistics);
                return showStat;
            }
        }
        private void ShowStatistics(object obj)
        {
            string path = "C:\\Users\\anaop\\source\\repos\\HangMan\\HangMan\\Model\\playerData.txt";
            string[] textFile = File.ReadAllLines(path);
            player = new Player(textFile[0], Int32.Parse(textFile[1]), Int32.Parse(textFile[2]));
            System.Windows.MessageBox.Show("Games played: " + player.games.ToString() + "\n" + "Games won: " + player.gamesWon, "Player statistics", MessageBoxButton.OK);
        }
            
        //------------------button functions--------
        private void ButtonPress(ref bool wasPressed, string lower, string upper)
        {
            if (startWasPressed == true)
            {
                if (wasPressed == false)
                {
                    if (wordGuess.Contains(lower) || wordGuess.Contains(upper))
                    {
                        wasPressed = true;
                        for (int i = 0; i < wordGuess.Length; i++)
                        {
                            if (wordGuess[i] == lower[0])
                            {
                                blankWord = blankWord.Remove(i, 1).Insert(i, lower);
                                OnPropertyChanged("blankWord");

                            }
                            else if (wordGuess[i] == upper[0])
                            {
                                blankWord = blankWord.Remove(i, 1).Insert(i, upper);
                                OnPropertyChanged("blankWord");
                            }
                            CheckForWin();
                        }
                    }
                    else
                    {
                        lives -= 1;
                        if (lives == 0)
                        {
                            blankWord = wordGuess;
                            hangmanIndex += 1;
                            currentHangman = hangman[hangmanIndex];
                            OnPropertyChanged("currentHangman");

                            string path2 = "C:\\Users\\anaop\\source\\repos\\HangMan\\HangMan\\Model\\playerData.txt";
                            string[] textFile = File.ReadAllLines(path2);
                            player = new Player(textFile[0], Int32.Parse(textFile[1]), Int32.Parse(textFile[2]));
                            player.games++;
                            score--;
                            SaveStatistics(player);
                            MessageBox.Show("YOU LOSE.Press EXIT to continue", "Lose message", MessageBoxButton.OK);
                        }
                        else if (lives > 0)
                        {
                            hangmanIndex += 1;
                            currentHangman = hangman[hangmanIndex];
                            OnPropertyChanged("currentHangman");
                        }
                        wasPressed = true;
                        OnPropertyChanged("lives");
                    }
                }
                else
                {
                    MessageBox.Show("This button was pressed already.Please choose another letter", "Error", MessageBoxButton.OK);
                }
            }
            else
            {
                MessageBox.Show("Start the timer before guessing!", "Error", MessageBoxButton.OK);
            }
        }
        private void CheckForWin()
        {
            if (blankWord == wordGuess)
            {
                score += 1;
                SaveStatistics(player);
                MessageBox.Show("YOU WIN THIS ROUND!", "WIN", MessageBoxButton.OK);
                GetNextGuessWord();

            }
            if (score == 5)
            {
                string path2 = "C:\\Users\\anaop\\source\\repos\\HangMan\\HangMan\\Model\\playerData.txt";
                string[] textFile = File.ReadAllLines(path2);
                player = new Player(textFile[0], Int32.Parse(textFile[1]), Int32.Parse(textFile[2]));
                player.games++;
                player.gamesWon++;
                SaveStatistics(player);
                MessageBox.Show("YOU WON THE GAME!", "WIN", MessageBoxButton.OK);
            }
        }

        bool startWasPressed;
        ICommand startButton;
        public ICommand StartCommand
        {
            get
            {
                if (startButton == null)
                    startButton = new RelayCommand(Start);
                return startButton;
            }
        }
        private void Start(object obj)
        {
            startWasPressed = true;
        }

        bool aWasPressed;
        ICommand aButton;
        public ICommand Acommand
        {
            get
            {
                if (aButton == null)
                    aButton = new RelayCommand(A);
                return aButton;
            }
        }
        private void A(object obj)
        {
            ButtonPress(ref aWasPressed, "a", "A");
        }

        bool bWasPressed;
        ICommand bButton;
        public ICommand Bcommand
        {
            get
            {
                if (bButton == null)
                    bButton = new RelayCommand(B);
                return bButton;
            }
        }
        private void B(object obj)
        {
            ButtonPress(ref bWasPressed, "b", "B");
        }

        bool cWasPressed;
        ICommand cButton;
        public ICommand Ccommand
        {
            get
            {
                if (cButton == null)
                    cButton = new RelayCommand(C);
                return cButton;
            }
        }
        private void C(object obj)
        {
            ButtonPress(ref cWasPressed, "c", "C");
        }


        bool dWasPressed;
        ICommand dButton;
        public ICommand Dcommand
        {
            get
            {
                if (dButton == null)
                    dButton = new RelayCommand(D);
                return dButton;
            }
        }
        private void D(object obj)
        {
            ButtonPress(ref dWasPressed, "d", "D");
        }


        bool eWasPressed;
        ICommand eButton;
        public ICommand Ecommand
        {
            get
            {
                if (eButton == null)
                    eButton = new RelayCommand(E);
                return eButton;
            }
        }
        private void E(object obj)
        {
            ButtonPress(ref eWasPressed, "e", "E");
        }


        bool fWasPressed;
        ICommand fButton;
        public ICommand Fcommand
        {
            get
            {
                if (fButton == null)
                    fButton = new RelayCommand(F);
                return fButton;
            }
        }
        private void F(object obj)
        {
            ButtonPress(ref fWasPressed, "f", "F");
        }


        bool gWasPressed;
        ICommand gButton;
        public ICommand Gcommand
        {
            get
            {
                if (gButton == null)
                    gButton = new RelayCommand(G);
                return gButton;
            }
        }
        private void G(object obj)
        {
            ButtonPress(ref gWasPressed, "g", "G");
        }


        bool hWasPressed;
        ICommand m_H;
        public ICommand Hcommand
        {
            get
            {
                if (m_H == null)
                    m_H = new RelayCommand(H);
                return m_H;
            }
        }
        private void H(object obj)
        {
            ButtonPress(ref hWasPressed, "h", "H");
        }


        bool iWasPressed;
        ICommand m_I;
        public ICommand Icommand
        {
            get
            {
                if (m_I == null)
                    m_I = new RelayCommand(I);
                return m_I;
            }
        }
        private void I(object obj)
        {
            ButtonPress(ref iWasPressed, "i", "I");
        }


        bool jWasPressed;
        ICommand m_J;
        public ICommand Jcommand
        {
            get
            {
                if (m_J == null)
                    m_J = new RelayCommand(J);
                return m_J;
            }
        }
        private void J(object obj)
        {
            ButtonPress(ref jWasPressed, "j", "J");
        }


        bool kWasPressed;
        ICommand m_K;
        public ICommand Kcommand
        {
            get
            {
                if (m_K == null)
                    m_K = new RelayCommand(K);
                return m_K;
            }
        }
        private void K(object obj)
        {
            ButtonPress(ref kWasPressed, "k", "K");
        }


        bool lWasPressed;
        ICommand m_L;
        public ICommand Lcommand
        {
            get
            {
                if (m_L == null)
                    m_L = new RelayCommand(L);
                return m_L;
            }
        }
        private void L(object obj)
        {
            ButtonPress(ref lWasPressed, "l", "L");
        }


        bool mWasPressed;
        ICommand m_M;
        public ICommand Mcommand
        {
            get
            {
                if (m_M == null)
                    m_M = new RelayCommand(M);
                return m_M;
            }
        }
        private void M(object obj)
        {
            ButtonPress(ref mWasPressed, "m", "M");
        }


        bool nWasPressed;
        ICommand m_N;
        public ICommand Ncommand
        {
            get
            {
                if (m_N == null)
                    m_N = new RelayCommand(N);
                return m_N;
            }
        }
        private void N(object obj)
        {
            ButtonPress(ref nWasPressed, "n", "N");
        }


        bool oWasPressed;
        ICommand m_O;
        public ICommand Ocommand
        {
            get
            {
                if (m_O == null)
                    m_O = new RelayCommand(O);
                return m_O;
            }
        }
        private void O(object obj)
        {
            ButtonPress(ref oWasPressed, "o", "O");

        }


        bool pWasPressed;
        ICommand m_P;
        public ICommand Pcommand
        {
            get
            {
                if (m_P == null)
                    m_P = new RelayCommand(P);
                return m_P;
            }
        }
        private void P(object obj)
        {
            ButtonPress(ref pWasPressed, "p", "P");
        }


        bool qWasPressed;
        ICommand m_Q;
        public ICommand Qcommand
        {
            get
            {
                if (m_Q == null)
                    m_Q = new RelayCommand(Q);
                return m_Q;
            }
        }
        private void Q(object obj)
        {
            ButtonPress(ref qWasPressed, "q", "Q");
        }


        bool rWasPressed;
        ICommand m_R;
        public ICommand Rcommand
        {
            get
            {
                if (m_R == null)
                    m_R = new RelayCommand(R);
                return m_R;
            }
        }
        private void R(object obj)
        {
            ButtonPress(ref rWasPressed, "r", "R");
        }


        bool sWasPressed;
        ICommand m_S;
        public ICommand Scommand
        {
            get
            {
                if (m_S == null)
                    m_S = new RelayCommand(S);
                return m_S;
            }
        }
        private void S(object obj)
        {
            ButtonPress(ref sWasPressed, "s", "S");
        }


        bool tWasPressed;
        ICommand m_T;
        public ICommand Tcommand
        {
            get
            {
                if (m_T == null)
                    m_T = new RelayCommand(T);
                return m_T;
            }
        }
        private void T(object obj)
        {
            ButtonPress(ref tWasPressed, "t", "T");
        }


        bool uWasPressed;
        ICommand m_U;
        public ICommand Ucommand
        {
            get
            {
                if (m_U == null)
                    m_U = new RelayCommand(U);
                return m_U;
            }
        }
        private void U(object obj)
        {
            ButtonPress(ref uWasPressed, "u", "U");
        }


        bool vWasPressed;
        ICommand m_V;
        public ICommand Vcommand
        {
            get
            {
                if (m_V == null)
                    m_V = new RelayCommand(V);
                return m_V;
            }
        }
        private void V(object obj)
        {
            ButtonPress(ref vWasPressed, "v", "V");

        }


        bool wWasPressed;
        ICommand m_W;
        public ICommand Wcommand
        {
            get
            {
                if (m_W == null)
                    m_W = new RelayCommand(W);
                return m_W;
            }
        }
        private void W(object obj)
        {
            ButtonPress(ref wWasPressed, "w", "W");

        }


        bool xWasPressed;
        ICommand m_X;
        public ICommand Xcommand
        {
            get
            {
                if (m_X == null)
                    m_X = new RelayCommand(X);
                return m_X;
            }
        }
        private void X(object obj)
        {
            ButtonPress(ref xWasPressed, "x", "X");
        }


        bool yWasPressed;
        ICommand m_Y;
        public ICommand Ycommand
        {
            get
            {
                if (m_Y == null)
                    m_Y = new RelayCommand(Y);
                return m_Y;
            }
        }
        private void Y(object obj)
        {
            ButtonPress(ref yWasPressed, "y", "Y");

        }

        bool zWasPressed;
        ICommand m_Z;
        public ICommand Zcommand
        {
            get
            {
                if (m_Z == null)
                    m_Z = new RelayCommand(Z);
                return m_Z;
            }
        }
        private void Z(object obj)
        {
            ButtonPress(ref zWasPressed, "z", "Z");
        }

    }
}
