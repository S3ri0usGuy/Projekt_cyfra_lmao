using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using Pathfinding.Ionic.Zip;

public class PlayerDialogsScript : MonoBehaviour
{
    public GameObject text;
    public GameObject panel;
    public Transform NPC1, NPC2, NPC3, NPC4;
    [SerializeField] public float triggerRadius;
    public TextMeshProUGUI dialog;

    private bool canTalk = false;
    private bool isTalking = false;
    private string dispalydText;
    private bool sceneIsActive;
    private bool sceneIsDone = false;
    [SerializeField] private Transform flowers, flowers2;
    [SerializeField] private GameObject page1, page2, page3;
    private int randomNumber;

    private Vector2 playerPosition;

    private PlayerResources playerResources;
    private Abilities abilities;

    private bool blaksmithIsDone = false, herbalistIsDone = false, musicianIsDone = false;

    [SerializeField] private GameObject questField;
    [SerializeField] private TextMeshProUGUI questText;
    [SerializeField] private GameObject portretMC, portretR, portretGP, portretB, portretZ, portretG;

    float distance1, distance2, distance3, distance4, flowersDistance, flowers2Distance, page1Distance, page2Distance, page3Distance;

    private int questCount = 0;
    [SerializeField] private int talker;
    [SerializeField] private int recentTalker = 0;
    [SerializeField] private int plotStage = 1;
    [SerializeField] private int stage;

    private void Start()
    {
        if (PlayerPrefs.HasKey("plotStage"))
        {
            plotStage = PlayerPrefs.GetInt("plotStage");
        }

        switch (plotStage)
        {
            case 6:
                questText.text = "Talk to the Grandpa.";
                break;
            case 10:
                questText.text = "Talk to the Blacksmith.";
                break;
            case 12:
                questText.text = "Talk to the Herbalist.";
                break;
            case 16:
                questText.text = "Talk to anyone to skip a day.";
                break;
            default:
                questText.text = "Error #1";
                break;
        }

        questField.SetActive(false);

        playerResources = GetComponent<PlayerResources>();
        abilities = GetComponent<Abilities>();

        text.SetActive(false);
        panel.SetActive(false);

        page1.SetActive(false);
        page2.SetActive(false);
        page3.SetActive(false);

        StartScene();
    }

    private void Update()
    {
        playerPosition = gameObject.transform.position;
        distance1 = Vector2.Distance(playerPosition, NPC1.position);
        distance2 = Vector2.Distance(playerPosition, NPC2.position);
        distance3 = Vector2.Distance(playerPosition, NPC3.position);
        distance4 = Vector2.Distance(playerPosition, NPC4.position);
        flowersDistance = Vector2.Distance(playerPosition, flowers.position);
        flowers2Distance = Vector2.Distance(playerPosition, flowers2.position);
        page1Distance = Vector2.Distance(playerPosition, page1.transform.position);
        page2Distance = Vector2.Distance(playerPosition, page2.transform.position);
        page3Distance = Vector2.Distance(playerPosition, page3.transform.position);

        if (((distance1 < triggerRadius && !isTalking) || (distance2 < triggerRadius && !isTalking) || (distance3 < triggerRadius && !isTalking) || (distance4 < triggerRadius && !isTalking) || sceneIsActive) && !playerResources.isNight)
            canTalk = true;
        else
            canTalk = false;

        if (distance1 < triggerRadius) talker = 1;
        else if (distance2 < triggerRadius) talker = 2;
        else if (distance3 < triggerRadius) talker = 3;
        else if (distance4 < triggerRadius) talker = 4;
        else if ((flowersDistance < triggerRadius * 2 || flowers2Distance < triggerRadius * 1.5) && plotStage == 8) talker = 5;
        else if (page1Distance < triggerRadius && plotStage == 13) talker = 6;
        else if (page2Distance < triggerRadius && plotStage == 13) talker = 7;
        else if (page3Distance < triggerRadius && plotStage == 13) talker = 8;
        else talker = 0;

        text.SetActive(canTalk);
    }
    
    public void StartScene()
    {
        sceneIsActive = true;
        stage = 0;
        canTalk = true;
        Talk();
        sceneIsActive = false;
    }

    public void Scene1()
    {
        switch (stage)
        {
            case 0:
                dispalydText = "Hey, are you listening?! We're almost there!";
                ChangePortret(2);
                break;
            case 1:
                dispalydText = "Mhm, yeah...";
                ChangePortret(1);
                break;
            case 2:
                dispalydText = "I can't wait to slay some monsters! I mean you will be the one to do that and I'll be just watching! We're gonna have a lot of fun!";
                ChangePortret(2);
                break;
            case 3:
                dispalydText = "Fun? What's so fun about that?";
                ChangePortret(1);
                break;
            case 4:
                dispalydText = "What's not fun about slaying monsters! And you're gonna be a new hero, saving the whole village!";
                ChangePortret(2);
                break;
            case 5:
                dispalydText = "That's just my job. Also, weren't the villagers supposed to never find out about the monsters?";
                ChangePortret(1);
                break;
            case 6:
                dispalydText = "You're right, mister... But it's still so exciting! A mysterious hero who doesn't feel the need to get praised!";
                ChangePortret(2);
                break;
            case 7:
                dispalydText = "Well, if that's how you wanna perceive it...";
                ChangePortret(1);
                break;
            case 8:
                dispalydText = "I wonder if it will end the same way again...";
                ChangePortret(2);
                break;
            case 9:
                dispalydText = "What do you mean?";
                ChangePortret(1);
                break;
            case 10:
                dispalydText = "Oh, nevermind! We're here!";
                ChangePortret(2);
                break;
            case 11:
                TogleTalking(false);
                plotStage++;
                questField.SetActive(true);
                questText.text = "Tlak to the Grandpa.";
                stage = 0;
                break;
        }
    }

    public void Scene2()
    {
        if (talker != 1)
        {
            switch (stage)
            {
                case 0:
                    dispalydText = "I don't think that's Grandpa...";
                    ChangePortret(2);
                    break;
                case 1:
                    TogleTalking(false);
                    break;
            }
            return;
        }
        switch (stage)
        {
            case 0:
                dispalydText = "Welcome to our village! I have been waiting for your arrival. My name is Edward.";
                ChangePortret(3);
                break;
            case 1:
                dispalydText = "Thank you. I'm Aiden.";
                ChangePortret(1);
                break;
            case 2:
                dispalydText = "I was very surprised when I found out we will have a new resident. Our village is rather small and quiet. What brings you here?";
                ChangePortret(3);
                break;
            case 3:
                dispalydText = "I just needed some change and peace in my life.";
                ChangePortret(1);
                break;
            case 4:
                dispalydText = "Wonderful! I hope you will find it here! If you have any questions, do not hesitate to ask me. I have lived here all my 70 years of life and I know everything about this village.";
                ChangePortret(3);
                break;
            case 5:
                dispalydText = "Thank you. I will remember that.";
                ChangePortret(1);
                break;
            case 6:
                dispalydText = "Now, everyone here knows each other. You should greet them when you find some time.";
                ChangePortret(3);
                break;
            case 7:
                dispalydText = "Of course, I will.";
                ChangePortret(1);
                break;
            case 8:
                plotStage++;
                questField.SetActive(false);
                TogleTalking(false);
                Scene3Initiator();
                playerResources.UseActions(1);
                break;
        }
    }

    public void Scene3()
    {
        if (talker == 1)
        {
            switch (stage)
            {
                case 0:
                    dispalydText = "You have already met him...";
                    ChangePortret(2);
                    break;
                case 1:
                    TogleTalking(false);
                    break;
            }
            return;
        }
        switch (stage)
        {
            case 0:
                dispalydText = "You're not very talkative, huh?";
                ChangePortret(2);
                break;
            case 1:
                dispalydText = "I don't like to waste my time on useless conversations. Now, let's just go home and wait for the night.";
                ChangePortret(1);
                break;
            case 2:
                dispalydText = "No, no, didn't you learn anything useful in the battle academy?!";
                ChangePortret(2);
                break;
            case 3:
                dispalydText = "Well, I learned how to fight.";
                ChangePortret(1);
                break;
            case 4:
                dispalydText = "Fighting is one thing, but you gotta think about connections! How will you get new weapons? How will you learn new spells? Who will help you if you get sick?";
                ChangePortret(2);
                break;
            case 5:
                dispalydText = "I will manage.";
                ChangePortret(1);
                break;
            case 6:
                dispalydText = "No, you don't understand! Nobody will help the suspicious guy who never gets out of his house. Listen to the old man and go meet the villagers.";
                ChangePortret(2);
                break;
            case 7:
                dispalydText = "Hmm. You may have a point here.";
                ChangePortret(1);
                break;
            case 8:
                dispalydText = "Of course I do! Let's go!";
                ChangePortret(2);
                break;
            case 9:
                TogleTalking(false);
                plotStage++;
                questField.SetActive(true);
                questText.text = "Meet villagers: " + questCount + " / 3";
                stage = 0;
                break;
        }
    }

    public void Scene4()
    {
        if (talker == 1 || (talker == 2 && blaksmithIsDone) || (talker == 3 && herbalistIsDone) || (talker == 4 && musicianIsDone))
        {
            switch (stage)
            {
                case 0:
                    dispalydText = "You have already met him...";
                    ChangePortret(2);
                    break;
                case 1:
                    TogleTalking(false);
                    break;
            }
            return;
        }
        else if (recentTalker == 2)
        {
            switch (stage)
            {
                case 0:
                    dispalydText = "Hehe, see, he likes you already!";
                    ChangePortret(2);
                    break;
                case 1:
                    dispalydText = "What did I tell you about talking around other people?";
                    ChangePortret(1);
                    break;
                case 2:
                    dispalydText = "Don't worry! They can't hear me! And I can't stand being stuffed in your bag for the whole day!";
                    ChangePortret(2);
                    break;
                case 3:
                    dispalydText = "I'd rather not hear you as well...";
                    ChangePortret(1);
                    break;
                case 4:
                    dispalydText = "So mean! Anyway, he already knows you're walking around with a plushie, there's no going back.";
                    ChangePortret(2);
                    break;
                case 5:
                    TogleTalking(false);
                    questCount++;
                    recentTalker = 0;
                    break;
            }
        }
        else if (recentTalker == 3)
        {
            switch (stage)
            {
                case 0:
                    dispalydText = "So they really don't teach social skills in the battle academy!";
                    ChangePortret(2);
                    break;
                case 1:
                    dispalydText = "Why did they need to send you with me...?";
                    ChangePortret(1);
                    break;
                case 2:
                    dispalydText = "Huh? You don't remember? Someone has to control how the state of the village is and whether you're doing your job properly.";
                    ChangePortret(2);
                    break;
                case 3:
                    dispalydText = "No, I meant... why you, specifically.";
                    ChangePortret(1);
                    break;
                case 4:
                    dispalydText = "So mean, again! You're gonna make me cry, mister! Just kidding, let's go!";
                    ChangePortret(2);
                    break;
                case 5:
                    TogleTalking(false);
                    questCount++;
                    recentTalker = 0;
                    break;
            }
        }
        else if (talker == 2)
        {
            switch (stage)
            {
                case 0:
                    dispalydText = "Oh, hello new face! Gramps was talking about the new resident for the whole week.";
                    ChangePortret(4);
                    break;
                case 1:
                    dispalydText = "Hello. My name is Aiden.";
                    ChangePortret(1);
                    break;
                case 2:
                    dispalydText = "Nice to meet you, my guy! I'm Ralph and I'm the blacksmith here. Well, I was. Before I found out it's a bit useless... So now I make pottery.";
                    ChangePortret(4);
                    break;
                case 3:
                    dispalydText = "Nice to meet you.";
                    ChangePortret(1);
                    break;
                case 4:
                    dispalydText = "<i>This guy making pottery? No way.<i>";
                    ChangePortret(1);
                    break;
                case 5:
                    dispalydText = "Hehe, that's a weird one!";
                    ChangePortret(2);
                    break;
                case 6:
                    dispalydText = "What's in your bag, my guy? Is that a plushie?";
                    ChangePortret(4);
                    break;
                case 7:
                    dispalydText = "Uh... No, it's...";
                    ChangePortret(1);
                    break;
                case 8:
                    dispalydText = "You seem like a serious guy but you've got a cute side it seems! I like you already! Feel free to come see me if you need any new plates or cups!";
                    ChangePortret(4);
                    break;
                case 9:
                    dispalydText = "Thank you. I will remember that.";
                    ChangePortret(2);
                    break;
                case 10:
                    TogleTalking(false);
                    blaksmithIsDone = true;
                    questText.text = "Meet villagers: " + (questCount + 1) + " / 3";
                    stage = 0;
                    Scene4dot2Initiator();
                    break;
            }
        }
        else if (talker == 3)
        {
            switch (stage)
            {
                case 0:
                    dispalydText = "Hello.";
                    ChangePortret(1);
                    break;
                case 1:
                    dispalydText = "...";
                    ChangePortret(5);
                    break;
                case 2:
                    dispalydText = "Hello?";
                    ChangePortret(1);
                    break;
                case 3:
                    dispalydText = "Oh, sorry. I got caught up in my own thoughts for a moment. I don't recognize your face. Are you the new resident everyone keeps talking about?";
                    ChangePortret(5);
                    break;
                case 4:
                    dispalydText = "Yes. My name is Aiden.";
                    ChangePortret(1);
                    break;
                case 5:
                    dispalydText = "My name is Bella. It's a pleasure to meet you.";
                    ChangePortret(5);
                    break;
                case 6:
                    dispalydText = "Nice to meet you too.";
                    ChangePortret(1);
                    break;
                case 7:
                    dispalydText = "...";
                    ChangePortret(5);
                    break;
                case 8:
                    dispalydText = "<i>Uhh... Is it my turn again?<i>";
                    ChangePortret(1);
                    break;
                case 9:
                    dispalydText = "Come on, say something!";
                    ChangePortret(2);
                    break;
                case 10:
                    dispalydText = "So… What do you do in the village?";
                    ChangePortret(1);
                    break;
                case 11:
                    dispalydText = "I'm a herbalist. I collect various herbs and plants and use them to make potions and medicine.";
                    ChangePortret(5);
                    break;
                case 12:
                    dispalydText = "That's... interesting.";
                    ChangePortret(1);
                    break;
                case 13:
                    dispalydText = "And you, why did you choose our village?";
                    ChangePortret(5);
                    break;
                case 14:
                    dispalydText = "I just needed a change and it seemed like a peaceful place to live.";
                    ChangePortret(1);
                    break;
                case 15:
                    dispalydText = "I see. I'm glad you are trying to get to know us. We rarely get new residents, but I hope you will find a new home in our village.";
                    ChangePortret(5);
                    break;
                case 16:
                    dispalydText = "Thank you.";
                    ChangePortret(1);
                    break;
                case 17:
                    TogleTalking(false);
                    herbalistIsDone = true;
                    questText.text = "Meet villagers: " + (questCount + 1) + " / 3";
                    stage = 0;
                    Scene4dot3Initiator();
                    break;
            }
        }
        else if (talker == 4)
        {
            switch (stage)
            {
                case 0:
                    dispalydText = "<i>He's playing... so terribly...<i>";
                    ChangePortret(1);
                    break;
                case 1:
                    dispalydText = "My ears! Walk faster!";
                    ChangePortret(2);
                    break;
                case 2:
                    dispalydText = "<i>I think I shouldn't interrupt him...<i>";
                    ChangePortret(1);
                    break;
                case 3:
                    TogleTalking(false);
                    musicianIsDone = true;
                    questCount++;
                    questText.text = "Meet villagers: " + questCount + " / 3";
                    stage = 0;
                    break;
            }
        }
        if(playerResources.actions == 2 && questCount == 1) playerResources.UseActions(1);
        if (questCount >= 3)
        {
            plotStage++;
            questCount = 0;
        }
        if (blaksmithIsDone && herbalistIsDone && musicianIsDone) Scene5Initiator();
    }

    public void Scene5()
    {
        if (talker == 1 || (talker == 2 && blaksmithIsDone) || (talker == 3 && herbalistIsDone) || (talker == 4 && musicianIsDone))
        {
            switch (stage)
            {
                case 0:
                    dispalydText = "You have already met him...";
                    ChangePortret(2);
                    break;
                case 1:
                    TogleTalking(false);
                    break;
            }
            return;
        }
        switch (stage)
        {
            case 0:
                dispalydText = "Phew, It was tough hearing you talk to everyone! But it's better than staying home all day.";
                ChangePortret(2);
                break;
            case 1:
                dispalydText = "I think the conversations went pretty well.";
                ChangePortret(1);
                break;
            case 2:
                dispalydText = "Yeah, because everyone is so nice here... But if you keep it up, good relationships with villagers can help you!";
                ChangePortret(2);
                break;
            case 3:
                dispalydText = "I hope you're right.";
                ChangePortret(1);
                break;
            case 4:
                dispalydText = "I'm always right! Now, you should go take a nap and prepare for the night.";
                ChangePortret(2);
                break;
            case 5:
                TogleTalking(false);
                plotStage++;
                stage = 0;
                playerResources.UseActions(1);
                PlayerPrefs.SetInt("plotStage", plotStage);
                questText.text = "Talk to the Grandpa.";
                questField.SetActive(false);
                break;
        }
    }
    
    /////////////////////////////////////////////////////////////////////////////////

    public void Scene6()
    {
        if (talker != 1)
        {
            switch (stage)
            {
                case 0:
                    dispalydText = "I don't think that's Grandpa...";
                    ChangePortret(2);
                    break;
                case 1:
                    TogleTalking(false);
                    break;
            }
            return;
        }
        switch (stage)
        {
            case 0:
                dispalydText = "Welcome, Aiden. How do you like our village so far?";
                ChangePortret(3);
                break;
            case 1:
                dispalydText = "It's nice. I met a few people and everyone seems friendly.";
                ChangePortret(1);
                break;
            case 2:
                dispalydText = "I am very happy to hear that. Our village still stands because everyone treats each other kindly.";
                ChangePortret(3);
                break;
            case 3:
                dispalydText = "And everyone is always willing to help their fellow neighbours. Actually, would you mind doing me a small favour?";
                ChangePortret(3);
                break;
            case 4:
                dispalydText = "What favour?";
                ChangePortret(1);
                break;
            case 5:
                dispalydText = "I always bring fresh flowers to my late wife's grave, but today I have a lot of work to do.";
                ChangePortret(3);
                break;
            case 6:
                dispalydText = "Would you please get a pretty bouquet from Bella and bring it to my dear wife's grave?";
                ChangePortret(3);
                break;
            case 7:
                dispalydText = "I mean... sure.";
                ChangePortret(1);
                break;
            case 8:
                dispalydText = "Everyone knows each other here, so I want you to meet my wife as well. She was a wonderful woman and did a lot for our village...";
                ChangePortret(3);
                break;
            case 9:
                dispalydText = "Sorry, you should get going now. I'm sure you will find Bella at her place, and the graveyard is on the northwest from here.";
                ChangePortret(3);
                break;
            case 10:
                dispalydText = "Sure, I will see you after I come back.";
                ChangePortret(1);
                break;
            case 11:
                plotStage++;
                TogleTalking(false);
                questText.text = "Talk to the Herbalist.";
                break;
        }
    }

    public void Scene7()
    {
        if (talker != 3)
        {
            switch (stage)
            {
                case 0:
                    dispalydText = "I don't think that's Herbalist...";
                    ChangePortret(2);
                    break;
                case 1:
                    TogleTalking(false);
                    break;
            }
            return;
        }
        switch (stage)
        {
            case 0:
                dispalydText = "Oh, hello. It's... Aiden, right?";
                ChangePortret(5);
                break;
            case 1:
                dispalydText = "Yes. Edward asked me to bring a flower bouquet to his wife's grave and told me that you should have one.";
                ChangePortret(1);
                break;
            case 2:
                dispalydText = "Really? He always goes there by himself. He must be really busy to ask someone to do that.";
                ChangePortret(5);
                break;
            case 3:
                dispalydText = "Or he wants you to really get to know our village... Either way, I'm sorry but I ran out of flowers. Could you collect them for me? I'll prepare other things in the meantime.";
                ChangePortret(5);
                break;
            case 4:
                dispalydText = "No problem. Where can I find them?";
                ChangePortret(5);
                break;
            case 5:
                dispalydText = "They're not that far, a bit to the east. The flowers are red and really pretty, so it's hard to miss them. I need around 7 of them.";
                ChangePortret(5);
                break;
            case 6:
                dispalydText = "Thank you. I will be back.";
                ChangePortret(1);
                break;
            case 7:
                plotStage++;
                TogleTalking(false);
                playerResources.UseActions(1);
                questText.text = "Collect red flowers: " + questCount + " / 7";
                break;
        }
    }

    public void Scene8()
    {
        if (questCount < 7)
        {
            switch (stage)
            {
                case 0:
                    dispalydText = "We need to find the flowers fist...";
                    ChangePortret(2);
                    break;
                case 1:
                    TogleTalking(false);
                    break;
            }
            return;
        }
        else if (talker != 3)
        {
            switch (stage)
            {
                case 0:
                    dispalydText = "I don't think that's Herbalist...";
                    ChangePortret(2);
                    break;
                case 1:
                    TogleTalking(false);
                    break;
            }
            return;
        }
        switch (stage)
        {
            case 0:
                dispalydText = "You're back already?";
                ChangePortret(5);
                break;
            case 1:
                dispalydText = "Yes, here are the flowers.";
                ChangePortret(1);
                break;
            case 2:
                dispalydText = "Oh, you cut them very nicely. I prepared everything already, so the bouquet will be ready in a few minutes.";
                ChangePortret(5);
                break;
            case 3:
                dispalydText = "Sure.";
                ChangePortret(1);
                break;
            case 4:
                dispalydText = "Did they teach you how to cut flowers nicely in the battle academy?";
                ChangePortret(2);
                break;
            case 5:
                dispalydText = "No, they taught me how to cut others... ";
                ChangePortret(1);
                break;
            case 6:
                dispalydText = "Here is the bouquet.";
                ChangePortret(5);
                break;
            case 7:
                dispalydText = "Thank you. It looks... pretty.";
                ChangePortret(1);
                break;
            case 8:
                dispalydText = "Wow, even he can say something nice!";
                ChangePortret(2);
                break;
            case 9:
                dispalydText = "I'm glad you think so. I made it with a lot of care because I respect Edward's late wife very much. She was the one who taught me floristry.";
                ChangePortret(5);
                break;
            case 10:
                dispalydText = "Was she a florist?";
                ChangePortret(1);
                break;
            case 11:
                dispalydText = "Not exactly. She was a teacher, she basically raised us when we were kids. But she was skilled in a lot of fields and I aspire to be like her.";
                ChangePortret(5);
                break;
            case 12:
                dispalydText = "She sure was a great person.";
                ChangePortret(1);
                break;
            case 13:
                dispalydText = "Yes, she really was. Now, take the flowers and pay her a visit.";
                ChangePortret(5);
                break;
            case 14:
                plotStage++;
                questCount = 0;
                TogleTalking(false);
                playerResources.UseActions(1);
                questText.text = "Go to the cemetery.";
                break;
        }
    }

    public void Scene9()
    {
        if (sceneIsDone)
        {
            switch (stage)
            {
                case 0:
                    dispalydText = "<i>There's only a few graves here. So this village hasn't been here for a long time.<i>";
                    ChangePortret(1);
                    break;
                case 1:
                    dispalydText = "<i>Each of them is kept so clean. Seems like the villagers really care about their ancestors.<i>";
                    ChangePortret(1);
                    break;
                case 2:
                    dispalydText = "<i>Sounds like something impossible for people like me... After we die, we're forgotten.<i>";
                    ChangePortret(1);
                    break;
                case 3:
                    dispalydText = "Hey, why are you standing here like that! Put the flowers and let's go.";
                    ChangePortret(2);
                    break;
                case 4:
                    dispalydText = "Oh, yeah, sure.";
                    ChangePortret(1);
                    break;
                case 5:
                    TogleTalking(false);
                    sceneIsDone = false;
                    questText.text = "Go back to the Grandpa.";
                    break;
            }
            return;
        }
        else if (questCount == 0)
        {
            switch (stage)
            {
                case 0:
                    dispalydText = "We need to find the cemetery...";
                    ChangePortret(2);
                    break;
                case 1:
                    TogleTalking(false);
                    break;
            }
            return;
        }
        else if (talker != 1)
        {
            switch (stage)
            {
                case 0:
                    dispalydText = "I don't think that's Grandpa...";
                    ChangePortret(2);
                    break;
                case 1:
                    TogleTalking(false);
                    break;
            }
            return;
        }
        switch (stage)
        {
            case 0:
                dispalydText = "Aiden, did you manage to find the grave?";
                ChangePortret(3);
                break;
            case 1:
                dispalydText = "Yes, I did.";
                ChangePortret(1);
                break;
            case 2:
                dispalydText = "Thank you. I'm sure my wife is happy that our new resident paid her a visit.";
                ChangePortret(3);
                break;
            case 3:
                dispalydText = "I'm glad to hear that.";
                ChangePortret(5);
                break;
            case 4:
                plotStage++;
                TogleTalking(false);
                abilities.playerHasability2 = true;
                PlayerPrefs.SetInt("playerHasability2", 1);
                PlayerPrefs.SetInt("plotStage", plotStage);
                playerResources.UseActions(1);
                questCount = 0;
                questText.text = "Talk to the Blacksmith.";
                break;
        }
    }

    /////////////////////////////////////////////////////////////////////////////////

    public void Scene10()
    {
        if (talker != 2)
        {
            switch (stage)
            {
                case 0:
                    dispalydText = "I don't think that's Blacksmith...";
                    ChangePortret(2);
                    break;
                case 1:
                    TogleTalking(false);
                    break;
            }
            return;
        }
        switch (stage)
        {
            case 0:
                dispalydText = "Welcome, my guy! How have you been doing?";
                ChangePortret(4);
                break;
            case 1:
                dispalydText = "I'm fine, thank you. What about you?";
                ChangePortret(1);
                break;
            case 2:
                dispalydText = "Pretty good! But something weird happened yesterday evening.";
                ChangePortret(4);
                break;
            case 3:
                dispalydText = "What happened?";
                ChangePortret(1);
                break;
            case 4:
                dispalydText = "I heard some weird noises before I went to sleep...";
                ChangePortret(4);
                break;
            case 5:
                dispalydText = "What? Did he hear us fighting monsters yesterday?!";
                ChangePortret(2);
                break;
            case 6:
                dispalydText = "Uhhh... Noises?";
                ChangePortret(1);
                break;
            case 7:
                dispalydText = "Yes! I couldn't have imagined them... It sounded like some people had a party... But if there was a party in the village, I would have known it!";
                ChangePortret(4);
                break;
            case 8:
                dispalydText = "Phew! He's not talking about us!";
                ChangePortret(2);
                break;
            case 9:
                dispalydText = "I think they came from the east... I was very sleepy so I still went to bed normally, so I'm not angry or anything!";
                ChangePortret(4);
                break;
            case 10:
                dispalydText = "But I want to know... I think I remember, there was a villa in the forest on the east... I saw it once before, but I thought nobody lives there.";
                ChangePortret(4);
                break;
            case 11:
                dispalydText = "Uh oh, we'd better check it! What if they hear us fighting monsters and come here at night?!";
                ChangePortret(2);
                break;
            case 12:
                dispalydText = "If you have some free time, my guy, would you take a walk there? You will also get to know our surroundings better. And I want to make sure I didn't imagine things!";
                ChangePortret(4);
                break;
            case 13:
                dispalydText = "Sure, I can check it.";
                ChangePortret(1);
                break;
            case 14:
                dispalydText = "Thanks! Come back to me after you're done.";
                ChangePortret(4);
                break;
            case 15:
                plotStage++;
                TogleTalking(false);
                playerResources.UseActions(1);
                questText.text = "Find a property in the northeast.";
                break;
        }
    }

    public void Scene11()
    {
        if (sceneIsDone)
        {
            switch (stage)
            {
                case 0:
                    dispalydText = "Wow, it's really here! And it's huge!";
                    ChangePortret(1);
                    break;
                case 1:
                    dispalydText = "Yeah. It definitely looks like someone lives there.";
                    ChangePortret(1);
                    break;
                case 2:
                    dispalydText = "Hey, do you think they could hear us at night?";
                    ChangePortret(1);
                    break;
                case 3:
                    dispalydText = "I gave it some thought and I doubt it. If Ralph heard the party from his house, why wouldn't we hear it being outside?";
                    ChangePortret(2);
                    break;
                case 4:
                    dispalydText = "Oh, you're kinda smart! That's a battle academy guy for you!";
                    ChangePortret(1);
                    break;
                case 5:
                    dispalydText = "Yeah, maybe people here really go to sleep so early the party lasted only until evening.";
                    ChangePortret(1);
                    break;
                case 6:
                    dispalydText = "That sounds like a boring party... Anyway, you can take a look around and let's come back to Ralph.";
                    ChangePortret(1);
                    break;
                case 7:
                    TogleTalking(false);
                    sceneIsDone = false;
                    playerResources.UseActions(1);
                    questText.text = "Go back to the Blacksmith.";
                    break;
            }
            return;
        }
        else if (questCount == 0)
        {
            switch (stage)
            {
                case 0:
                    dispalydText = "We need to find the property...";
                    ChangePortret(2);
                    break;
                case 1:
                    TogleTalking(false);
                    break;
            }
            return;
        }
        else if (talker != 2)
        {
            switch (stage)
            {
                case 0:
                    dispalydText = "I don't think that's Blacksmith...";
                    ChangePortret(2);
                    break;
                case 1:
                    TogleTalking(false);
                    break;
            }
            return;
        }
        switch (stage)
        {
            case 0:
                dispalydText = "Aiden! Did you see the villa?";
                ChangePortret(4);
                break;
            case 1:
                dispalydText = "Yeah, it really is there and it looks like someone lives there.";
                ChangePortret(1);
                break;
            case 2:
                dispalydText = "See, I was right! Now I'm curious about who that is! Aren't you?";
                ChangePortret(4);
                break;
            case 3:
                dispalydText = "Oh, yeah. I am.";
                ChangePortret(1);
                break;
            case 4:
                dispalydText = "They probably haven't lived there for a long time. We should go and welcome them soon! Even though the villa is not a part of our village, we're basically neighbours!";
                ChangePortret(4);
                break;
            case 5:
                dispalydText = "What do you say?";
                ChangePortret(4);
                break;
            case 6:
                dispalydText = "Uh, yeah, we can go greet them someday...";
                ChangePortret(1);
                break;
            case 7:
                dispalydText = "Why do you sound so unsure! Don't worry, you're a nice guy, they will like you!";
                ChangePortret(4);
                break;
            case 8:
                dispalydText = "Anyway, thanks for today! You helped me check out the villa. I will ask you when I come up with a plan to welcome them nicely!";
                ChangePortret(4);
                break;
            case 9:
                dispalydText = "Sure. No problem.";
                ChangePortret(1);
                break;
            case 10:
                dispalydText = "Take care, my guy!";
                ChangePortret(4);
                break;
            case 11:
                dispalydText = "Hehe, good job Mr. Nice Guy!";
                ChangePortret(2);
                break;
            case 12:
                dispalydText = "...";
                ChangePortret(1);
                break;
            case 13:
                plotStage++;
                TogleTalking(false);
                abilities.playerHasBow = true;
                PlayerPrefs.SetInt("playerHasBow", 1);
                PlayerPrefs.SetInt("plotStage", plotStage);
                playerResources.UseActions(1);
                questCount = 0;
                questText.text = "Talk to the Herbalist.";
                break;
        }
    }

    /////////////////////////////////////////////////////////////////////////////////

    public void Scene12()
    {
        if (talker != 3)
        {
            switch (stage)
            {
                case 0:
                    dispalydText = "I don't think that's Herbalist...";
                    ChangePortret(2);
                    break;
                case 1:
                    TogleTalking(false);
                    break;
            }
            return;
        }
        switch (stage)
        {
            case 0:
                dispalydText = "...";
                ChangePortret(5);
                break;
            case 1:
                dispalydText = "Hello?";
                ChangePortret(1);
                break;
            case 2:
                dispalydText = "Oh. Welcome, Aiden.";
                ChangePortret(5);
                break;
            case 3:
                dispalydText = "Did something happen? You look troubled.";
                ChangePortret(1);
                break;
            case 4:
                dispalydText = "Well... I was supposed to make some medicine for Edward, but it's rather complicated to make, so I have every step written on paper.";
                ChangePortret(5);
                break;
            case 5:
                dispalydText = "But... I put all the papers here for a moment and the wind blew away most of them... I was running around collecting them, but it seems like I'm still missing three pieces.";
                ChangePortret(5);
                break;
            case 6:
                dispalydText = "Do you want me to look for them?";
                ChangePortret(1);
                break;
            case 7:
                dispalydText = "Would you? That would help me a lot. I think the wind blew them quite far, because I couldn't see them anywhere nearby...";
                ChangePortret(5);
                break;
            case 8:
                dispalydText = "No problem. I'll walk around and see if I can find them.";
                ChangePortret(1);
                break;
            case 9:
                dispalydText = "Thank you.";
                ChangePortret(5);
                break;
            case 10:
                plotStage++;
                TogleTalking(false);
                playerResources.UseActions(1);
                page1.SetActive(true);
                page2.SetActive(true);
                page3.SetActive(true);
                questText.text = "Find all recipe pages: " + questCount + " / 3";
                break;
        }
    }

    public void Scene13()
    {
        if (questCount < 3)
        {
            switch (stage)
            {
                case 0:
                    dispalydText = "We need to find all the pages fist...";
                    ChangePortret(2);
                    break;
                case 1:
                    TogleTalking(false);
                    break;
            }
            return;
        }
        else if (talker != 3)
        {
            switch (stage)
            {
                case 0:
                    dispalydText = "I don't think that's Herbalist...";
                    ChangePortret(2);
                    break;
                case 1:
                    TogleTalking(false);
                    break;
            }
            return;
        }
        switch (stage)
        {
            case 0:
                dispalydText = "You found them all? Thank you. I hope it wasn't too much trouble.";
                ChangePortret(5);
                break;
            case 1:
                dispalydText = "It’s fine.";
                ChangePortret(1);
                break;
            case 2:
                dispalydText = "Would you mind waiting for a moment and bringing the medicine to Edward?";
                ChangePortret(5);
                break;
            case 3:
                dispalydText = "No problem.";
                ChangePortret(1);
                break;
            case 4:
                dispalydText = "...";
                ChangePortret(2);
                break;
            case 5:
                dispalydText = "...";
                ChangePortret(1);
                break;
            case 6:
                dispalydText = "Here it is, tell Edward to drink it right after a meal. And come back to me after, please.";
                ChangePortret(5);
                break;
            case 7:
                dispalydText = "Sure.";
                ChangePortret(1);
                break;
            case 8:
                plotStage++;
                questCount = 0;
                TogleTalking(false);
                playerResources.UseActions(1);
                questText.text = "Give the potion to Grandpa.";
                break;
        }
    }

    public void Scene14()
    {
        if (talker != 1)
        {
            switch (stage)
            {
                case 0:
                    dispalydText = "I don't think that's Grandpa...";
                    ChangePortret(2);
                    break;
                case 1:
                    TogleTalking(false);
                    break;
            }
            return;
        }
        switch (stage)
        {
            case 0:
                dispalydText = "Oh, it's Aiden.";
                ChangePortret(5);
                break;
            case 1:
                dispalydText = "I brought you medicine from Bella. She said you should drink it right after eating.";
                ChangePortret(1);
                break;
            case 2:
                dispalydText = "You brought it for me? Thank you. I really needed this one for my backache. You know, I may not look like it, but I am actually quite old.";
                ChangePortret(5);
                break;
            case 3:
                dispalydText = "He definitely does look like it!";
                ChangePortret(5);
                break;
            case 4:
                dispalydText = "No problem. I'll be going.";
                ChangePortret(5);
                break;
            case 5:
                dispalydText = "Take care and thank Bella for me as well.";
                ChangePortret(5);
                break;
            case 6:
                plotStage++;
                TogleTalking(false);
                questText.text = "Go back to the Herbalist";
                break;
        }
    }

    public void Scene15()
    {
        if (talker != 3)
        {
            switch (stage)
            {
                case 0:
                    dispalydText = "I don't think that's Herbalist...";
                    ChangePortret(2);
                    break;
                case 1:
                    TogleTalking(false);
                    break;
            }
            return;
        }
        switch (stage)
        {
            case 0:
                dispalydText = "Did you bring him the potion?";
                ChangePortret(5);
                break;
            case 1:
                dispalydText = "Yes, he told me to thank you.";
                ChangePortret(1);
                break;
            case 2:
                dispalydText = "Oh, it's fine. I want to thank you for all this trouble.";
                ChangePortret(5);
                break;
            case 4:
                plotStage++;
                TogleTalking(false);
                abilities.playerHasability3 = true;
                PlayerPrefs.SetInt("playerHasability3", 1);
                PlayerPrefs.SetInt("plotStage", plotStage);
                playerResources.UseActions(1);
                questText.text = "Talk to anyone to skip a day.";
                break;
        }
    }

    /////////////////////////////////////////////////////////////////////////////////

    public void Scene16()
    {
        switch (stage)
        {
            case 0:
                dispalydText = "I wolud like to skip a day!";
                ChangePortret(1);
                randomNumber = Random.Range(1, 21);
                break;
            case 1:
                if (randomNumber != 20) dispalydText = "Sure thing!";
                else dispalydText = "Smoke gum!";
                ChangePortret(talker + 2);
                break;
            case 2:
                TogleTalking(false);
                playerResources.UseActions(3);
                break;
        }
    }

    private void Scene3Initiator()
    {
        if (distance1 >= triggerRadius * 2) StartScene();
        else Invoke("Scene3Initiator", 0.2f);
    }

    private void Scene4dot2Initiator()
    {
        recentTalker = 2;
        if (distance2 >= triggerRadius * 2) StartScene();
        else Invoke("Scene4dot2Initiator", 0.2f);
    }

    private void Scene4dot3Initiator()
    {
        recentTalker = 3;
        if (distance3 >= triggerRadius * 2) StartScene();
        else Invoke("Scene4dot3Initiator", 0.2f);
    }

    private void Scene5Initiator()
    {
        if ((distance2 >= triggerRadius * 3) && (distance3 >= triggerRadius * 3) && (distance4 >= triggerRadius * 3) && (distance1 >= triggerRadius * 3)) StartScene();
        else Invoke("Scene5Initiator", 0.2f);
    }

    public void Talk()
    {
        if (talker == 5 && plotStage == 8)
        {
            questCount++;
            questText.text = "Collect red flowers: " + questCount + " / 7";
        }
        else if (talker >= 6 && plotStage == 13)
        {
            if(talker == 6) page1.SetActive(false);
            else if(talker == 7) page2.SetActive(false);
            else if(talker == 8) page3.SetActive(false);
            questCount++;
            questText.text = "Find all recipe pages: " + questCount + " / 3";
        }
        if (playerResources.isNight) return;
        if (sceneIsDone) canTalk = true;
        if (!canTalk && !isTalking) return;
        if (!isTalking) TogleTalking(true);

        switch (plotStage)
        {
            case 1:
                Scene1();
                break;
            case 2:
                Scene2();
                break;
            case 3:
                Scene3();
                break;
            case 4:
                Scene4();
                break;
            case 5:
                Scene5();
                break;
            case 6:
                Scene6();
                break;
            case 7:
                Scene7();
                break;
            case 8:
                Scene8();
                break;
            case 9:
                Scene9();
                break;
            case 10:
                Scene10();
                break;
            case 11:
                Scene11();
                break;
            case 12:
                Scene12();
                break;
            case 13:
                Scene13();
                break;
            case 14:
                Scene14();
                break;
            case 15:
                Scene15();
                break;
            case 16:
                Scene16();
                break;
            default: break;
        }

        dialog.text = dispalydText;
        stage++;
    }

    public void TogleTalking(bool startTaling)
    {
        isTalking = startTaling;
        panel.SetActive(startTaling);
        if(startTaling)
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        else
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        stage = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("SceneTrigger") && plotStage == 9)
        {
            questCount = 1;
            sceneIsDone = true;
            Talk();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("SceneTrigger2") && plotStage == 11)
        {
            questCount = 1;
            sceneIsDone = true;
            Talk();
            Destroy(other.gameObject);
        }
    }

    private void ChangePortret(int n)
    {
        portretMC.SetActive(false);
        portretR.SetActive(false);
        portretGP.SetActive(false);
        portretB.SetActive(false);
        portretZ.SetActive(false);
        portretG.SetActive(false);
        switch (n)
        {
            case 1:
                portretMC.SetActive(true);
                break;
            case 2:
                portretR.SetActive(true);
                break;
            case 3:
                portretGP.SetActive(true);
                break;
            case 4:
                portretB.SetActive(true);
                break;
            case 5:
                portretZ.SetActive(true);
                break;
            case 6:
                portretG.SetActive(true);
                break;
        }
    }
}
