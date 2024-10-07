using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Helping class that works as a storage for all possible walls and their coding in bit values
// 4 bits for simple walls like up down left right walls
// 8 bits for complex walls like L-shape to right or L-shape to left

public class WallBitValues
{
    public static HashSet<int> TopWall = new()
    {
        0b0010,
        0b0011,
        0b1010,
        0b0110,
        0b0111,
        0b1110,
        0b1011,
        0b1111,
    };

    public static HashSet<int> LeftWall = new()
    {
        0b0100,
    };

    public static HashSet<int> LeftWall8Dir = new()
    {
        0b00000101,
        0b01000100,
        0b01000101,
    };

    public static HashSet<int> RightWall = new()
    {
        0b0001,
    };

    public static HashSet<int> RightWall8Dir = new()
    {
        0b00010001,
        0b01010000,
        0b01010001,
    };

    public static HashSet<int> BottomWall = new()
    {
        0b1000,
    };

    public static HashSet<int> RightTopCorner = new()
    {
        0b00000100,
    };

    public static HashSet<int> LeftTopCorner = new()
    {
        0b00010000,
    };

    public static HashSet<int> RightBottomCorner = new()
    {
        0b00000001,
    };

    public static HashSet<int> LeftBottomCorner = new()
    {
        0b01000000,
    };

    public static HashSet<int> LCornerRight = new()
    {
        0b11110001,
        0b11100000,
        0b11110000,
        0b11100001,
        0b10100000,
        0b11010000,
        0b10110001,
        0b10100001,
        0b10010000,
        0b10110000,
        0b10010001,
        0b11010001,
    };

    public static HashSet<int> LCornerLeft = new()
    {
        0b11000111,
        0b11000011,
        0b10000011,
        0b10000111,
        0b10000010,
        0b11000101,
        0b10000101,
        0b11000110,
        0b11000010,
        0b10000100,
        0b10000110,
        0b11000100,
    };

    public static HashSet<int> SinglePeak = new()
    {
        0b11100011,
        0b11100111,
        0b11110011,
        0b11110111,
        0b10100111,
        0b11010010,
        0b11010100,
        0b11010111,
        0b11010011,
        0b10010111,
        0b11110110,
        0b11100010,
        0b11100101,
        0b11010101,
        0b10100100,
        0b11110100,
        0b11010110,
        0b11110101,
        0b10100011,
        0b10110111,
        0b11110010,
        0b10110100,
        0b11100100,
        0b10110101,
        0b11100110,
        0b10010011,
        0b10010110,
        0b10010010,
        0b10110110,
        0b10100110,
        0b10100101,
        0b10100010,
        0b10010100,
        0b10110011,
    };

    public static HashSet<int> SinglePeakConnector = new()
    {
        0b00010100,
        0b00010110,
        0b00100110,
        0b00110100,
        0b00110110,
        0b00110111,
        0b01100111,
        0b01100011,
        0b01110111,
        0b00100100,
        0b00100111,
        0b01100010,
        0b01110011,
        0b00100011,
        0b01110110,
        0b00010010,
        0b00110010,
        0b00010011,
        0b01110100,
        0b01010100,
        0b01100110,
        0b00100010,
        0b01010110,
        0b01100101,
        0b00010111,
        0b00100101,
        0b00110011,
        0b01110101,
        0b00010101,
        0b01010101,
        0b01110010,
        0b01010111,
        0b01100100,
    };
}
