# CZ4001-VR

This game is developed in AY2019-20 Sem 2 for CZ4001 Virtual and Augmented Reality.

![hunting wallpaper](Hunting/Assets/Wallpaper/Deer-Hunting-Wallpapers.jpg)

## Versions

- Unity: 2018.3.1f1

## Set-Up

after cloning:

1. [configure Unity for version control](https://thoughtbot.com/blog/how-to-git-with-unity)
2. copy the following folders from 'Hunting' (from NTULearn) into the folder 'Library':
    - [BFW]SimpleDynamicClouds
    - Animal Pack Deluxe
    - Free Island Collection
    - NaturalTilingTextures
    - PostProcessing
    - Standard Assets
    - SteamVR
    - TextMeshPro
    - Tree10
    - Tribal Jungle Music Free Pack
    - VRTK
3. import the package 'Living Birds' from the Asset Store
4. move the downloaded folder ('living birds') into Library

note - the following VR-related GameObjects are disabled in the hierarchy so that the game can run within Unity editor on computers without VR support:

- [VRTK_SDKManager]
- [VRTK_Scripts]
- HeadsetFollower
- GameController/Bow (but not GameController itself)

## Building / Deploying

before building for VR (for CZ4001 computer):

- disable `Temp/TempPlayer`
- disable `Temp/TempGameManager`
- enable `[VRTK_SDKManager]`
- enable `[VRTK_Scripts]`
- enable `HeadsetFollower`
- enable `GameController/Bow`
- set the `Render Camera` parameter for `HUDCanvas` to `[VRTK_SDKManager]/SteamVR/[CameraRig]/Camera (head)/Camera (eye)`

when building for VR (for CZ4001 computer):

- select `PC, Mac & Linux Standalone`
- set `Target Platform` to `Windows`
- set `Architecture` to `x86_64`

## Resources used

- [birds](https://assetstore.unity.com/packages/3d/characters/animals/living-birds-15649)
- [reticle sprite](https://www.hiclipart.com/free-transparent-background-png-clipart-mryvr)

## Miscellaneous

### fixes

Remote "origin" does not support the LFS locking API. Consider disabling it with:

```bash
git config lfs.https://github.com/NgoJunHaoJason/CZ4001-VR.git/info/lfs.locksverify false
```
