#!/usr/bin/env python3
import time
import pygame as pg
from pygame.locals import *
from helpers import draw_image, draw_text, lf, tc

pg.init()

FPS = 60
FramePerSec = pg.time.Clock()
surface = pg.display.set_mode()

timer = 0

pg.display.set_caption("Confronting Yourself")

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

class LinearTransform:
    start_pos: pg.Vector2
    end_pos: pg.Vector2
    duration: float
    start_time: float | None
    is_complete: bool
    is_started: bool

    def __init__(self, start_pos: pg.Vector2, end_pos: pg.Vector2, duration: float):
        self.start_pos = start_pos
        self.end_pos = end_pos
        self.duration = duration
        self.start_time = None
        self.is_complete = False
        self.is_started = False

    def begin(self):
        self.start_time = timer
        self.is_complete = False
        self.is_started = True

    def update(self) -> tuple[pg.Vector2, float]:
        if not self.is_started:
            return self.start_pos, 0.0
        if self.start_time is None or self.is_complete:
            return self.end_pos, 1.0

        elapsed = timer - self.start_time
        t = min(elapsed / self.duration, 1.0)

        current_pos = pg.Vector2(
            self.start_pos.x + (self.end_pos.x - self.start_pos.x) * t,
            self.start_pos.y + (self.end_pos.y - self.start_pos.y) * t,
        )

        if t >= 1.0:
            self.is_complete = True

        return current_pos, t

target_x = 270

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

confronting_timer.begin()
redcircle_timer.begin()

def draw_title():
    global confronting_timer, redcircle_timer, yourself_timer

    if timer >= 300.0 and not yourself_timer.is_started:
        yourself_timer.begin()
    if timer >= 600.0 and not act1_timer.is_started:
        act1_timer.begin()

    draw_red_circle(redcircle_timer.update()[0])
    draw_confronting(confronting_timer.update()[0])
    draw_yourself(yourself_timer.update()[0])
    draw_act1(act1_timer.update()[0])

started = False

while True:
    for event in pg.event.get():
        if event.type == QUIT:
            pg.quit()
            break

    surface.fill((0, 0, 0))

    draw_title()

    dt = FramePerSec.get_fps() / 1000

    draw_text(surface, fonts["sonic"], f"FPS: {FramePerSec.get_fps():.2f}", pg.Vector2(0, 30))
    draw_text(surface, fonts["sonic"], f"timer: {timer}", pg.Vector2(0, 70))
    draw_text(surface, fonts["sonic"], f"DT: {dt:.4f}", pg.Vector2(0, 110))

    pg.display.update()
    FramePerSec.tick(FPS)
    timer += FramePerSec.get_time()
