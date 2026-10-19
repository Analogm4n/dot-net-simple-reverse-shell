#!/usr/bin/env python3
import argparse

def ip_to_hex_bytes(ip_address):
    # Convierte cada carácter de la IP a su valor hexadecimal ASCII
    hex_bytes = [f"0x{ord(char):02x}" for char in ip_address]
    return ", ".join(hex_bytes)

def main():
    parser = argparse.ArgumentParser(
        description="Convierte una dirección IP en una matriz de bytes hexadecimales para C#.",
        formatter_class=argparse.RawTextHelpFormatter,
        epilog="Ejemplo de uso:\n  python3 ip_encoder.py 192.168.1.1\n"
    )
    
    parser.add_argument(
        "ip", 
        type=str, 
        nargs="?", 
        help="La dirección IP que deseas codificar (ej. 192.168.1.130)"
    )
    
    args = parser.parse_args()
    
    if not args.ip:
        parser.print_help()
        return

    formatted_bytes = ip_to_hex_bytes(args.ip)
    
    print("\n[+] Resultado listo para copiar:\n")
    print(f"static readonly string HOST = Encoding.ASCII.GetString(new byte[]{{")
    print(f"    {formatted_bytes}")
    print(f"}});\n")

if __name__ == "__main__":
    main()
