import os
import pygame as pg
from pygame.font import Font
from pygame.rect import Rect

ASSETS_BASE = os.path.join(os.path.dirname(os.path.abspath(__file__)), "assets/")

def tc(p: str) -> pg.Surface:
    img = pg.image.load(os.path.join(ASSETS_BASE, "title-card", p)).convert_alpha()
    return img

def lbg(p: str) -> pg.Surface:
    img = pg.image.load(os.path.join(ASSETS_BASE, "bg", p)).convert_alpha()
    return img

def lf(p: str, size: int) -> Font:
    return pg.font.Font(os.path.join(ASSETS_BASE, "fonts", p), size)


def draw_image(surface: pg.Surface, image: pg.Surface, pos: pg.Vector2, scale: tuple[float, float] = (1, 1)):
    new_w = image.get_width() * scale[0]
    new_h = image.get_height() * scale[1]
    image = pg.transform.scale(image, (new_w, new_h))
    rect: Rect = image.get_rect()
    rect = rect.move(pos)
    surface.blit(image, rect)
    # pg.draw.rect(
    #     surface,
    #     (255, 255, 255),
    #     rect, 1
    # )

def draw_text(surface: pg.Surface, font: Font, text: str, pos: pg.Vector2 = pg.Vector2(0, 0), color = (255, 255, 255)):
    fps_render = font.render(text, False, color)
    surface.blit(fps_render, (pos.x, pos.y))


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

    def begin(self, timer: float):
        self.start_time = timer
        self.is_complete = False
        self.is_started = True

    def update(self, timer: float) -> tuple[pg.Vector2, float]:
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
