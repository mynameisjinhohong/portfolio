using System.Collections.Generic;
using UnityEngine;

public class CardManager : Singleton<CardManager>
{
    //ī�� ��� ���� ��ũ��Ʈ
    public CardObject_HJH cardList;
    public GameObject Idx6Effect;
    public GameObject Idx7Effect;
    public GameObject Idx8Effect;
    public TileObject_PCI trapPrefab;
    public int seed;
    public void OnCardStart(Transform player, int cardIdx)
    {
        if (cardIdx < 0)
        {
            switch (cardIdx)
            {
                case -1:
                    if (cardIdx < 0)
                    {
                        Idx0EnforceFunc(player);
                    }
                    break;
                case -2:
                    if (cardIdx < 0)
                    {
                        Idx1EnforceFunc(player);
                    }
                    break;
                case -3:
                    if (cardIdx < 0)
                    {
                        Idx2EnforceFunc(player);
                    }
                    break;
                case -4:
                    if (cardIdx < 0)
                    {
                        Idx3EnforceFunc(player);
                    }
                    break;
                case -5:
                    if (cardIdx < 0)
                    {
                        Idx5EnforceFunc(player);
                    }
                    break;
                case -6:
                    if (cardIdx < 0)
                    {
                        Idx6EnforceFunc(player);
                    }
                    break;
                case -7:
                    if (cardIdx < 0)
                    {
                        Idx7EnforceFunc(player);
                    }
                    break;
                case -8:
                    if (cardIdx < 0)
                    {
                        Idx8EnforceFunc(player);
                    }
                    break;
                case -9:
                    if (cardIdx < 0)
                    {
                        Idx9EnforceFunc(player.GetComponent<Player_HJH>());
                    }
                    break;
                case -10:
                    Idx10EnforceFunc(player);
                    break;
                case -11:
                    Idx11EnforceFunc(player);
                    break;
            }
        }
        else
        {
            switch (cardIdx)
            {
                case 1:
                    Idx0Func(player);
                    break;
                case 2:
                    Idx1Func(player);
                    break;
                case 3:
                    Idx2Func(player);
                    break;
                case 4:
                    Idx3Func(player);
                    break;
                case 5:
                    Idx5Func(player);
                    break;
                case 6:
                    Idx6Func(player);
                    break;
                case 7:
                    Idx7Func(player);
                    break;
                case 8:
                    Idx8Func(player);
                    break;
                case 9:
                    Idx9Func(player.GetComponent<Player_HJH>());
                    break;
                case 10:
                    Idx10Func(player);
                    break;
                case 11:
                    Idx11Func(player);
                    break;
            }
        }


    }

    //ī�尡 ��� ��������
    public bool OnCardCheck(Player_HJH player, int cardIdx)
    {
        bool ret = true;
        switch (Mathf.Abs(cardIdx))
        {
            case 1:
                if (cardIdx < 0)
                {
                    ret = GamePlayManager.Instance.gameBoard.IsPathable(new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y + 2));
                }
                else
                {
                    ret = GamePlayManager.Instance.gameBoard.IsPathable(new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y + 1));
                }
                break;
            case 2:
                if (cardIdx < 0)
                {
                    ret = GamePlayManager.Instance.gameBoard.IsPathable(new Vector2Int((int)player.transform.position.x + 2, (int)player.transform.position.y));
                }
                else
                {
                    ret = GamePlayManager.Instance.gameBoard.IsPathable(new Vector2Int((int)player.transform.position.x + 1, (int)player.transform.position.y));
                }
                break;
            case 3:
                if (cardIdx < 0)
                {
                    ret = GamePlayManager.Instance.gameBoard.IsPathable(new Vector2Int((int)player.transform.position.x - 2, (int)player.transform.position.y));
                }
                else
                {
                    ret = GamePlayManager.Instance.gameBoard.IsPathable(new Vector2Int((int)player.transform.position.x - 1, (int)player.transform.position.y));
                }
                break;
            case 4:
                if (cardIdx < 0)
                {
                    ret = GamePlayManager.Instance.gameBoard.IsPathable(new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y - 2));
                }
                else
                {
                    ret = GamePlayManager.Instance.gameBoard.IsPathable(new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y - 1));
                }
                break;
            case 6:
                if (cardIdx < 0)
                {
                    ret = false;
                    for (int i = -2; i < 3; i++)
                    {
                        for (int j = -2; j < 3; j++)
                        {
                            if (GamePlayManager.Instance.gameBoard.IsPathable(new Vector2Int((int)player.transform.position.x + j, (int)player.transform.position.y + i)))
                            {
                                ret = true;
                            }
                        }
                    }
                }
                else
                {
                    ret = false;
                    for (int i = -1; i < 2; i++)
                    {
                        for (int j = -1; j < 2; j++)
                        {
                            if (GamePlayManager.Instance.gameBoard.IsPathable(new Vector2Int((int)player.transform.position.x + j, (int)player.transform.position.y + i)))
                            {
                                ret = true;
                            }
                        }
                    }
                }
                break;
        }
        return ret;

    }

    //�÷��̾� ���� 1ĭ �̵�
    void Idx0Func(Transform player)
    {
        Player_HJH p;
        Chaser c;
        if (player.TryGetComponent<Player_HJH>(out p))
        {
            if (GamePlayManager.Instance.gameBoard.IsPathable(new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y + 1)))
            {
                GamePlayManager.Instance.gameBoard.Interact(new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y + 1), p);
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z);
            }
            
            p.animator.Play("Walk");
        }
        else if(player.TryGetComponent<Chaser>(out c))
        {
            if (GamePlayManager.Instance.gameBoard.IsPathable(new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y + 1)))
            {
                GamePlayManager.Instance.gameBoard.InteractChaser(new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y + 1), c);
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z);
            }
        }
        AudioPlayer.Instance.PlayClip(8);
    }
    //�÷��̾� ���� 2ĭ �̵�
    void Idx0EnforceFunc(Transform player)
    {
        if (GamePlayManager.Instance.gameBoard.IsPathable(new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y + 2)))
        {
            GamePlayManager.Instance.gameBoard.Interact(new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y + 2), player.GetComponent<Player_HJH>());
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 2, player.transform.position.z);
        }
        Player_HJH p;
        if (player.TryGetComponent<Player_HJH>(out p))
        {
            p.animator.Play("Walk");
        }
        AudioPlayer.Instance.PlayClip(9);

    }
    //�÷��̾� ���������� 1ĭ �̵�
    void Idx1Func(Transform player)
    {
        Player_HJH p;
        Chaser c;
        if (player.TryGetComponent<Player_HJH>(out p))
        {
            if (GamePlayManager.Instance.gameBoard.IsPathable(new Vector2Int((int)player.transform.position.x + 1, (int)player.transform.position.y)))
            {
                GamePlayManager.Instance.gameBoard.Interact(new Vector2Int((int)player.transform.position.x + 1, (int)player.transform.position.y), p);
                player.transform.position = new Vector3(player.transform.position.x + 1, player.transform.position.y, player.transform.position.z);
            }
            
            p.animator.Play("Walk");
            p.sr.flipX = true;
        }
        else if(player.TryGetComponent<Chaser>(out c))
        {
            if (GamePlayManager.Instance.gameBoard.IsPathable(new Vector2Int((int)player.transform.position.x + 1, (int)player.transform.position.y)))
            {
                GamePlayManager.Instance.gameBoard.InteractChaser(new Vector2Int((int)player.transform.position.x + 1, (int)player.transform.position.y), c);
                player.transform.position = new Vector3(player.transform.position.x + 1, player.transform.position.y, player.transform.position.z);
            }
            
            c.chaserSR.flipX = true;
        }
        
        AudioPlayer.Instance.PlayClip(8);

    }
    //�÷��̾� ���������� 2ĭ �̵�
    void Idx1EnforceFunc(Transform player)
    {
        if (GamePlayManager.Instance.gameBoard.IsPathable(new Vector2Int((int)player.transform.position.x + 2, (int)player.transform.position.y)))
        {
            GamePlayManager.Instance.gameBoard.Interact(new Vector2Int((int)player.transform.position.x + 2, (int)player.transform.position.y), player.GetComponent<Player_HJH>());
            player.transform.position = new Vector3(player.transform.position.x + 2, player.transform.position.y, player.transform.position.z);

        }
        Player_HJH p;
        if (player.TryGetComponent<Player_HJH>(out p))
        {
            p.animator.Play("Walk");
            p.sr.flipX = true;

        }
        AudioPlayer.Instance.PlayClip(9);

    }
    //�÷��̾� �������� 1ĭ �̵�
    void Idx2Func(Transform player)
    {
        Player_HJH p;
        Chaser c;
        if (player.TryGetComponent<Player_HJH>(out p))
        {
            if (GamePlayManager.Instance.gameBoard.IsPathable(new Vector2Int((int)player.transform.position.x - 1, (int)player.transform.position.y)))
            {
                GamePlayManager.Instance.gameBoard.Interact(new Vector2Int((int)player.transform.position.x - 1, (int)player.transform.position.y), p);
                player.transform.position = new Vector3(player.transform.position.x - 1, player.transform.position.y, player.transform.position.z);
            }
            
            p.animator.Play("Walk");
            p.sr.flipX = false;
        }
        else if (player.TryGetComponent<Chaser>(out c))
        {
            if (GamePlayManager.Instance.gameBoard.IsPathable(new Vector2Int((int)player.transform.position.x - 1, (int)player.transform.position.y)))
            {
                GamePlayManager.Instance.gameBoard.InteractChaser(new Vector2Int((int)player.transform.position.x - 1, (int)player.transform.position.y), c);
                player.transform.position = new Vector3(player.transform.position.x - 1, player.transform.position.y, player.transform.position.z);
            }
            
            c.chaserSR.flipX = false;
        }
        
        AudioPlayer.Instance.PlayClip(8);

    }
    //�÷��̾� �������� 2ĭ �̵�
    void Idx2EnforceFunc(Transform player)
    {
        if (GamePlayManager.Instance.gameBoard.IsPathable(new Vector2Int((int)player.transform.position.x - 2, (int)player.transform.position.y)))
        {
            GamePlayManager.Instance.gameBoard.Interact(new Vector2Int((int)player.transform.position.x - 2, (int)player.transform.position.y), player.GetComponent<Player_HJH>());
            player.transform.position = new Vector3(player.transform.position.x - 2, player.transform.position.y, player.transform.position.z);
        }
        Player_HJH p;
        if (player.TryGetComponent<Player_HJH>(out p))
        {
            p.animator.Play("Walk");
            p.sr.flipX = false;

        }
        AudioPlayer.Instance.PlayClip(9);

    }
    //�÷��̾� �Ʒ��� 1ĭ �̵�
    void Idx3Func(Transform player)
    {
        Player_HJH p;
        Chaser c;
        
        if (player.TryGetComponent<Player_HJH>(out p))
        {
            if (GamePlayManager.Instance.gameBoard.IsPathable(new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y - 1)))
            {
                GamePlayManager.Instance.gameBoard.Interact(new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y - 1), p);
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 1, player.transform.position.z);
            }
            
            p.animator.Play("Walk");
        }
        else if (player.TryGetComponent<Chaser>(out c))
        {
            if (GamePlayManager.Instance.gameBoard.IsPathable(new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y - 1)))
            {
                GamePlayManager.Instance.gameBoard.InteractChaser(new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y - 1), c);
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 1, player.transform.position.z);
            }
        }
        
        AudioPlayer.Instance.PlayClip(8);

    }
    //�÷��̾� �Ʒ��� 2ĭ �̵�
    void Idx3EnforceFunc(Transform player)
    {
        if (GamePlayManager.Instance.gameBoard.IsPathable(new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y - 2)))
        {
            GamePlayManager.Instance.gameBoard.Interact(new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y - 2), player.GetComponent<Player_HJH>());
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 2, player.transform.position.z);
        }
        Player_HJH p;
        if (player.TryGetComponent<Player_HJH>(out p))
        {
            p.animator.Play("Walk");
        }
        AudioPlayer.Instance.PlayClip(9);

    }
    // �÷��̾ 3*3 �ȿ��� �����ϰ� �̵�
    void Idx5Func(Transform player)
    {
        Vector3 playerTransform = player.transform.position;
        Random.InitState(seed);
        int[] dx = { -1, 0, 1, -1, 1, -1, 0, 1 };
        int[] dy = { -1, -1, -1, 0, 0, 1, 1, 1 };
        List<int> list = new List<int>();
        for(int i = 0; i < 8; i++)
        {
            if(GamePlayManager.Instance.gameBoard.IsPathable(new Vector2Int((int)player.transform.position.x + dx[i], (int)player.transform.position.y + dy[i])))
            {
                list.Add(i);
            }
        }
        int rand = Random.Range(0, list.Count);
        GamePlayManager.Instance.gameBoard.Interact(new Vector2Int((int)player.transform.position.x + dx[list[rand]], (int)player.transform.position.y + dy[list[rand]]), player.GetComponent<Player_HJH>());
        player.transform.position = new Vector3(player.transform.position.x + dx[list[rand]], player.transform.position.y + dy[list[rand]], player.transform.position.z);

        Player_HJH p;
        if (player.TryGetComponent<Player_HJH>(out p))
        {
            GameObject shadow = Instantiate(p.playerShadow, playerTransform, Quaternion.identity);
            shadow.GetComponent<PlayerShadow_HJH>().StartFadeOut(p.animator);
            p.animator.Play("Walk");
            if (dx[rand] < 0) p.sr.flipX = false;
            else p.sr.flipX = true;
        }
        AudioPlayer.Instance.PlayClip(10);

    }
    // �÷��̾ 5*5 �ȿ��� �����ϰ� �̵�
    void Idx5EnforceFunc(Transform player)
    {
        Vector3 playerTransform = player.transform.position;
        Random.InitState(seed);
        int[] dx = {-2, -1, 0, 1, 2, -2, -1, 0, 1, 2, -2, -1, 1, 2, -2, -1, 0, 1, 2, -2, -1, 0, 1, 2};
        int[] dy = {-2, -2, -2, -2, -2, -1, -1, -1, -1, -1, 0, 0, 0, 0, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2};
        List<int> list = new List<int>();
        for (int i = 0; i < 24; i++)
        {
            if (GamePlayManager.Instance.gameBoard.IsPathable(new Vector2Int((int)player.transform.position.x + dx[i], (int)player.transform.position.y + dy[i])))
            {
                list.Add(i);
            }
        }
        int rand = Random.Range(0, list.Count);
        GamePlayManager.Instance.gameBoard.Interact(new Vector2Int((int)player.transform.position.x + dx[list[rand]], (int)player.transform.position.y + dy[list[rand]]), player.GetComponent<Player_HJH>());
        player.transform.position = new Vector3(player.transform.position.x + dx[list[rand]], player.transform.position.y + dy[list[rand]], player.transform.position.z);
        
        Player_HJH p;
        if (player.TryGetComponent<Player_HJH>(out p))
        {
            GameObject shadow = Instantiate(p.playerShadow, playerTransform, Quaternion.identity);
            shadow.GetComponent<PlayerShadow_HJH>().StartFadeOut(p.animator);
            p.animator.Play("Walk");
            
            if (dx[rand] < 0) p.sr.flipX = false;
            else p.sr.flipX = true;
        }
        AudioPlayer.Instance.PlayClip(10);

    }
    // �÷��̾ ���Ʒ� ����
    void Idx6Func(Transform player)
    {
        Vector2Int up = new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y + 1);
        Vector2Int down = new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y - 1);
        GamePlayManager.Instance.gameBoard.Attack(new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y + 1), player.GetComponent<Player_HJH>());
        GamePlayManager.Instance.gameBoard.Attack(new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y - 1), player.GetComponent<Player_HJH>());
        GamePlayManager.Instance.GoDamage(up, 1);
        GamePlayManager.Instance.GoDamage(down, 1);
        Instantiate(Idx6Effect, player);
        Player_HJH p;
        if (player.TryGetComponent<Player_HJH>(out p))
        {
            p.animator.Play("SwingVertical");
        }
        AudioPlayer.Instance.PlayClip(4);

    }
    // �÷��̾ ���Ʒ� ���ϰ԰���
    void Idx6EnforceFunc(Transform player)
    {
        Vector2Int up = new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y + 1);
        Vector2Int down = new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y - 1);
        GamePlayManager.Instance.gameBoard.Attack(new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y + 1), player.GetComponent<Player_HJH>());
        GamePlayManager.Instance.gameBoard.Attack(new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y - 1), player.GetComponent<Player_HJH>());
        Instantiate(Idx6Effect, player);
        GamePlayManager.Instance.GoDamage(up, 2);
        GamePlayManager.Instance.GoDamage(down, 2);
        Player_HJH p;
        if (player.TryGetComponent<Player_HJH>(out p))
        {
            p.animator.Play("SwingVertical");
        }
        AudioPlayer.Instance.PlayClip(5);

    }
    // �÷��̾ �¿� ����
    void Idx7Func(Transform player)
    {
        Vector2Int right = new Vector2Int((int)player.transform.position.x + 1, (int)player.transform.position.y);
        Vector2Int left = new Vector2Int((int)player.transform.position.x - 1, (int)player.transform.position.y);
        GamePlayManager.Instance.gameBoard.Attack(new Vector2Int((int)player.transform.position.x + 1, (int)player.transform.position.y), player.GetComponent<Player_HJH>());
        GamePlayManager.Instance.gameBoard.Attack(new Vector2Int((int)player.transform.position.x - 1, (int)player.transform.position.y), player.GetComponent<Player_HJH>());
        GamePlayManager.Instance.GoDamage(right, 1);
        GamePlayManager.Instance.GoDamage(left, 1);
        Instantiate(Idx7Effect, player);
        Player_HJH p;
        if (player.TryGetComponent<Player_HJH>(out p))
        {
            p.animator.Play("SwingHorizontal");
        }
        AudioPlayer.Instance.PlayClip(4);

    }
    // �÷��̾ �¿� ���ϰ԰���
    void Idx7EnforceFunc(Transform player)
    {
        Vector2Int right = new Vector2Int((int)player.transform.position.x + 1, (int)player.transform.position.y);
        Vector2Int left = new Vector2Int((int)player.transform.position.x - 1, (int)player.transform.position.y);
        GamePlayManager.Instance.gameBoard.Attack(new Vector2Int((int)player.transform.position.x + 1, (int)player.transform.position.y), player.GetComponent<Player_HJH>());
        GamePlayManager.Instance.gameBoard.Attack(new Vector2Int((int)player.transform.position.x - 1, (int)player.transform.position.y), player.GetComponent<Player_HJH>());
        GamePlayManager.Instance.GoDamage(right, 2);
        GamePlayManager.Instance.GoDamage(left, 2);
        Instantiate(Idx7Effect, player);
        Player_HJH p;
        if (player.TryGetComponent<Player_HJH>(out p))
        {
            p.animator.Play("SwingHorizontal");
        }
        AudioPlayer.Instance.PlayClip(5);

    }
    // �÷��̾ ��� ���� ����
    void Idx8Func(Transform player)
    {
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }
                Vector2Int vec = new Vector2Int((int)player.transform.position.x + i, (int)player.transform.position.y + j);
                GamePlayManager.Instance.gameBoard.Attack(vec, player.GetComponent<Player_HJH>());
                GamePlayManager.Instance.GoDamage(vec, 1);
            }
        }
        Instantiate(Idx8Effect, player);
        Player_HJH p;
        Chaser c;
        
        if (player.TryGetComponent<Player_HJH>(out p))
        {
            p.animator.Play("SmashDown");
        }
        else if (player.TryGetComponent<Chaser>(out c))
        {
            c.animator.Play("SmashDown");
        }
        
        AudioPlayer.Instance.PlayClip(6);

    }
    // �÷��̾ ������ ���ϰ԰���
    void Idx8EnforceFunc(Transform player)
    {
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }
                Vector2Int vec = new Vector2Int((int)player.transform.position.x + i, (int)player.transform.position.y + j);
                GamePlayManager.Instance.gameBoard.Attack(vec, player.GetComponent<Player_HJH>());
                GamePlayManager.Instance.GoDamage(vec, 2);
            }
        }
        Instantiate(Idx8Effect, player);
        Player_HJH p;
        Chaser c;
        if (player.TryGetComponent<Player_HJH>(out p))
        {
            p.animator.Play("SmashDown");
        }
        else if (player.TryGetComponent<Chaser>(out c))
        {
            c.animator.Play("SmashDown");
        }
        AudioPlayer.Instance.PlayClip(6);
    }
    //3�� �ȿ� ���� ������ ����
    void Idx9Func(Player_HJH player)
    {
        player.ShieldOn(3);
    }
    //5�� �ȿ� ���� ������ ����
    void Idx9EnforceFunc(Player_HJH player)
    {
        player.ShieldOn(5);
    }
    //���� ��ġ
    void Idx10Func(Transform player)
    {
        GamePlayManager.Instance.gameBoard.SetTile(trapPrefab, (int)player.transform.position.x, (int)player.transform.position.y,3);
    }
    //��ȭ ���� ��ġ
    void Idx10EnforceFunc(Transform player)
    {
        GamePlayManager.Instance.gameBoard.SetTile(trapPrefab, (int)player.transform.position.x, (int)player.transform.position.y, 5);
    }
    //ī�� 2�� �̱�
    void Idx11Func(Transform player)
    {
        if (player.gameObject.GetComponent<Player_HJH>().isMine)
        {
            GamePlayManager.Instance.playerDeck.DrawOne();
            GamePlayManager.Instance.playerDeck.DrawOne();
        }
    }
    //ī�� 3�� �̱�
    void Idx11EnforceFunc(Transform player)
    {
        if (player.gameObject.GetComponent<Player_HJH>().isMine)
        {
            GamePlayManager.Instance.playerDeck.DrawOne();
            GamePlayManager.Instance.playerDeck.DrawOne();
            GamePlayManager.Instance.playerDeck.DrawOne();
        }
    }
}
