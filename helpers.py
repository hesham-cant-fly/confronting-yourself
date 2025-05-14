import os
import pygame as pg
from pygame.font import Font
from pygame.rect import Rect

ASSETS_BASE = os.path.join(os.path.dirname(os.path.abspath(__file__)), "assets/")

def tc(p: str) -> pg.Surface:
    img = pg.image.load(os.path.join(ASSETS_BASE, "title-card", p)).convert_alpha()
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
