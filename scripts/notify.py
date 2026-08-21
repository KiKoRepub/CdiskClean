#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Windows 右下角通知工具
用法:
    notify.exe "请求喝水"              → 标题默认"通知"
    notify.exe -t "提醒" "请求喝水"     → 自定义标题
    notify.exe "提醒|请求喝水"          → 用 | 分隔标题和内容
"""
from win10toast import ToastNotifier
import sys
import argparse

def notify(title, message, duration=5):
    toaster = ToastNotifier()
    toaster.show_toast(title, message, duration=duration, threaded=False)

def main():
    parser = argparse.ArgumentParser(description="Windows 右下角通知工具")
    parser.add_argument("message", nargs="?", help="通知内容（或用 标题|内容 格式）")
    parser.add_argument("-t", "--title", default="通知", help="通知标题（默认: 通知）")
    parser.add_argument("-d", "--duration", type=int, default=5, help="显示时长秒数（默认: 5）")
    args = parser.parse_args()

    if not args.message:
        parser.print_help()
        sys.exit(1)

    # 解析 标题|内容 格式
    msg = args.message.strip()
    if "|" in msg:
        parts = msg.split("|", 1)
        title = parts[0].strip() or args.title
        content = parts[1].strip()
    else:
        title = args.title
        content = msg

    notify(title, content, args.duration)

if __name__ == "__main__":
    main()