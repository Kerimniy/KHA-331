import math
from typing import List

class Other:
    @staticmethod
    def num_sum(a: float) -> float:
        sum_ = 0
        a_s = str(a)
        for i in range(len(a_s)):
            sum_ += float(ord(a_s[i]))
        return sum_

    @staticmethod
    def byte_medium(b: List[int]) -> int:
        sum_ = sum(b)
        medium = int(sum_ / len(b))
        return medium % 256 

    @staticmethod
    def last_3_symbols(a: str) -> str:
        shifted = float(a)
        loged_str = ""
        if len(str(shifted)) > 3:
            loged_str = str(shifted)[-3:]
        else:
            loged_str = str(shifted)
            while len(loged_str) < 3:
                loged_str = "0" + loged_str
        return loged_str

   

class NSConvert:
    @staticmethod
    def to_base(number: int, base: int) -> str:
        if base == 0 or base > 36:
            return "OutOfDegreeRange"
        
        charbase = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        output = ""
        decimal_ = number
        while decimal_ > 0:
            output = charbase[int(decimal_ % base)] + output
            decimal_ = math.floor(decimal_ / base)

        new_output = output.lstrip("0") if output else "0"
        return new_output

    @staticmethod
    def from_base(input_str: str, base: int) -> int:
        if base == 0 or base > 36:
            return 1
        if len(input_str) > 32:
            input_str = input_str[:32]
        
        charbase = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        output = 0
        dynamic_array = []
        for char in input_str:
            for i, base_char in enumerate(charbase):
                if base_char == char.upper():
                    dynamic_array.append(i)
        
        for i, value in enumerate(dynamic_array):
            j = len(dynamic_array) - i
            output += value * int(math.pow(base, j - 1))
        
        return output

class Cryptography:
    @staticmethod
    def get_hash_code(input_str: str) -> str:
        text = input_str
        bytes_ = [0] * 64
        output_bytes = [0] * 32
        text_bytes = list(text.encode('utf-8'))
        output = ""

        
        for i in range(min(len(text_bytes), 64)):
            bytes_[i] = text_bytes[i]

        for j in range(len(text), 64):
            s = int(round(j * len(text) / 64.0))
            j1 = 0
            if s != 0 and j > 9:
                j1 = j // int(round(s * 1.5)) // 2
            else:
                j1 = j // 2 if j > 16 else j
            
            try:
                shifted = (bytes_[j - 1] << j1) ^ int(Other.num_sum(bytes_[j - 2]))
                shifted = shifted ^ int(Other.last_3_symbols(str(j * j * j)))
            except Exception:
                shifted = (bytes_[s] << (j1 + s)) ^ Other.byte_medium(bytes_)
            
            loged_str = ""
            if len(str(shifted)) > 3:
                loged_str = str(shifted)[-3:]
            else:
                loged_str = str(shifted)
                while len(loged_str) < 3:
                    loged_str = "0" + loged_str
            
            shifted = int(loged_str)
            while shifted > 255:
                shifted -= shifted * 0.6
                shifted = round(shifted)
            
            bytes_[j] = int(shifted) % 256

        for i in range(32):
            multiplied = bytes_[i] * bytes_[i + 32]
            while multiplied > 255:
                multiplied -= multiplied * 0.6
                multiplied = round(multiplied)
            output_bytes[i] = int(multiplied) % 256

        for el in output_bytes:
            output = NSConvert.to_base(el, 36) + output
        
        return output

