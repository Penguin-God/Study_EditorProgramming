using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Class
{
    Warrior,
    Magician,
}

[Serializable] // 프로퍼티를 내 맘대로 그리고 싶으면 일단 직렬화가 가능해야 함( 사실 컴포넌트에 Data를 뛰우고 싶으면 직렬화가 필수여서 별 상관없긴 함 )
public class PlayerData
{
    public int _level;
    public string _name;
    public Class _class;
}
