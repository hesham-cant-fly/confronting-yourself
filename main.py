#!/usr/bin/env python3
import Intro
import time
import pygame as pg
from pygame.locals import *
from helpers import LinearTransform, draw_image, draw_text, lf, tc, lbg

pg.init()

FPS = 60
FramePerSec = pg.time.Clock()
surface = pg.display.set_mode()

timer = 0

def get_timer() -> int:
    return timer

pg.display.set_caption("Confronting Yourself")

backgrounds = {
    "aiback": lbg("aiback.png"),
    "aifloor": lbg("aifloor.png"),
    "aigreen": lbg("aigreen.png"),
}

title_card = {
    "act1": tc("act1.png"),
    "confronting": tc("confronting.png"),
    "redcircle": tc("redcircle.png"),
    "yourself": tc("yourself.png"),
}

fonts = {
    "sonic": lf("sonic.ttf", 40),
    "sonicdebug": lf("sonicdebugfont.ttf", 40),
    "sonichud": lf("sonichud.ttf", 40),
}

def draw_red_circle(pos: pg.Vector2):
    draw_image(
        surface,
        title_card["redcircle"],
        pos,
        (4.4, 4.4),
    )

def draw_confronting(pos: pg.Vector2):
    draw_image(
        surface,
        title_card["confronting"],
        pos,
        (4.4, 4.4),
    )

def draw_yourself(pos: pg.Vector2):
    draw_image(
        surface,
        title_card["yourself"],
        pos,
        (4.4, 4.4),
    )

def draw_act1(pos: pg.Vector2):
    draw_image(
        surface,
        title_card["act1"],
        pos,
        (4.4, 4.4),
    )

confronting_timer = LinearTransform(
    pg.Vector2(-2000, 10),
    pg.Vector2(270, 10),
    500.0,
)
redcircle_timer = LinearTransform(
    pg.Vector2(2000, 10),
    pg.Vector2(270, 10),
    500.0,
)
yourself_timer = LinearTransform(
    pg.Vector2(-2000, 10),
    pg.Vector2(270, 10),
    500.0,
)
act1_timer = LinearTransform(
    pg.Vector2(2000, 10),
    pg.Vector2(270, 10),
    500.0,
)
black_op = 255

confronting_timer.begin(get_timer())
redcircle_timer.begin(get_timer())

_ending = False

def process_title():
    global confronting_timer, redcircle_timer, yourself_timer, act1_timer
    timer = get_timer()

    huh = pg.Surface((surface.get_width(), surface.get_height()), pg.SRCALPHA)
    huh.fill((0, 0, 0, black_op))

    surface.blit(huh, (0, 0))

    draw_red_circle(redcircle_timer.update(timer)[0])
    draw_confronting(confronting_timer.update(timer)[0])
    draw_yourself(yourself_timer.update(timer)[0])
    draw_act1(act1_timer.update(timer)[0])

def process_intro():
    global confronting_timer, redcircle_timer, yourself_timer, act1_timer, _ending, black_op

    if get_timer() >= 300.0 and not yourself_timer.is_started:
        yourself_timer.begin(timer)
    if get_timer() >= 600.0 and not act1_timer.is_started:
        act1_timer.begin(timer)
    if not _ending and get_timer() >= 2500.0:
        confronting_timer = LinearTransform(
            pg.Vector2(270, 10),
            pg.Vector2(-2000, 10),
            500.0,
        )
        redcircle_timer = LinearTransform(
            pg.Vector2(270, 10),
            pg.Vector2(2000, 10),
            500.0,
        )
        yourself_timer = LinearTransform(
            pg.Vector2(270, 10),
            pg.Vector2(-2000, 10),
            500.0,
        )
        act1_timer = LinearTransform(
            pg.Vector2(270, 10),
            pg.Vector2(2000, 10),
            500.0,
        )
        confronting_timer.begin(get_timer())
        redcircle_timer.begin(get_timer())
        yourself_timer.begin(get_timer())
        act1_timer.begin(get_timer())
        _ending = True
    if get_timer() >= 2000.0:
        black_op -= 10
        if black_op < 0:
            black_op = 0
    process_title()

backgrounds["aifloor"] = pg.transform.scale(backgrounds["aifloor"], (surface.get_width() * 1.5, surface.get_height() * 1.5))
if __name__ == "__main__":
    while True:
        for event in pg.event.get():
            if event.type == QUIT:
                pg.quit()
                break

        surface.fill((0, 0, 0))
        surface.blit(backgrounds["aifloor"], (0, -170))

        process_intro()

        dt = FramePerSec.get_fps() / 1000

        draw_text(surface, fonts["sonic"], f"FPS: {FramePerSec.get_fps():.2f}", pg.Vector2(0, 30))
        draw_text(surface, fonts["sonic"], f"TIMER: {timer}", pg.Vector2(0, 70))
        draw_text(surface, fonts["sonic"], f"DT: {dt:.4f}", pg.Vector2(0, 110))

        pg.display.update()
        FramePerSec.tick(FPS)
        timer += FramePerSec.get_time()
