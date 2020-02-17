# LiSiLity Arm
## Unity version 2018.3.2f1 - Upper Extremity Prosthetics Simulator - Controllable via Lab Streaming Layer (LSL) 
### Upper Extremity Simulator for Unity 
#### An Argzero Project S.T.A.T.I.C. repository
[in-progress]

If you are interested in collaborating on a research project using the LiSiLity Arm, feel free to contact me at:
forrest@argzero.org. If the license on this repo is not sufficient for your application, please contact me to discuss alternative arrangements. Any use of this repository will require, at a minimum, citation of Forrest Shooster or other contributors to Project STATIC & invitation to Forrest Z. Shooster to participate in any publications of research papers utilizing the tool.

I am open to any collaboration involving publications with anyone interested and with any tools they have which are compatible with my technologies. If you have an AI / ML algorithm utilizing LSL control or which has a C#, C++, or python plugin for interfacing, your tool is likely compatible with Project STATIC or will be soon. Please indicate to me if your project has other requirements; I may be willing to create a new interface for you or at least provide you necessary support so that you can build your own.

Supported Interfaces:
- Joint Angle Inputs & Outputs
- Finger Angle Inputs & Outputs (COMPLETE)
- Hand States / Custom State Input (COMPLETE) & Output
- AZ Thermoglove (Multiplexed Thermistor Array Glove) Input (COMPLETE) and Simulated Output
- Independent Control of Both Arms (COMPLETE)
- Transradial Component (COMPLETE)
- Transhumeral Component 

Currently, only LSL inputs and outputs are supported. Planned additions:
- SQLite support (unsecure, encryption, FIPS 140-2 compliance, online/offline)
- Text file / Custom Editor file
- ROS (if deemed achievable)

Current Planned Additions to Project STATIC for 2020:
- Fully functional built application for non-technical users
- Addition of lower extremity transtibial and/or ankle-foot prosthetic simulation to Project STATIC
- LSL Stream Control interface & settings subwindow
- Compartmentalized Systems and Subsystems for improved modularity
- Creation and publication of a database of existing prosthetic devices (meta-analysis of research and commercial devices)
- Review of device standards to determine specifications for revision of simulations to meet standard requirements
- Identification of important factors and creation of a basic searchable page describing prosthetics components, design, and references to existing devices
- Project STATIC web page
- Commenting and documentation of all code files

NOTE: This has not been tested to work in an earlier version of Unity or later versions of Unity. To get 2018.3.2f1, please refer to the Unity download archive for the correct Unity version. I personally recommend you use the Unity Hub Beta to manage your various Unity versions, found here: 
https://unity3d.com/get-unity/download

For the Unity version archive, look here: https://unity3d.com/get-unity/download/archive
