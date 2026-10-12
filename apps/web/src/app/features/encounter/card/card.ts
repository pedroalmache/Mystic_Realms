import {
  AfterViewInit,
  Component,
  ElementRef,
  HostListener,
  Input,
  NgZone,
  OnChanges,
  Renderer2,
  SimpleChanges,
  output,
} from '@angular/core';

import { CardDisplayComponent } from '../../../shared/card-display/card-display';
import {
  CARD_DISCARD_FADE_MS,
  CARD_DISCARD_HAND_FLIGHT_MS,
  CARD_PLAY_DELAY_MS,
} from '../../../core/constants/animation.constants';
import { Spirit } from '../../../core/services/single-player.service';

@Component({
  selector: 'app-card',
  standalone: true,
  templateUrl: './card.html',
  styleUrl: './card.scss',
  imports: [CardDisplayComponent],
})
export class CardComponent implements AfterViewInit, OnChanges {
  @Input() id: string = '';
  @Input() borderColor: string = 'black';
  @Input() characterId: number = 0;
  @Input() cardId: number = 0;
  @Input() name: string = '';
  @Input() description: string = '';
  @Input() cardIndex: number = 0;
  @Input() manaCost: number = 0;
  @Input() mana: number = 0;
  @Input() totalCards: number = 0;
  @Input() attack: number = 0;
  @Input() armor: number = 0;
  @Input() magicAttack: number = 0;
  @Input() magicArmor: number = 0;
  @Input() defense: number = 0;
  @Input() healing: number = 0;
  @Input() poison: number = 0;
  @Input() isMyTurn: boolean = false;
  @Input() playerEnergy: number = 0;
  @Input() rarity: number = 0;
  @Input() permanent: boolean = false;
  @Input() draw: number = 0;
  @Input() strength: number = 0;
  @Input() effectMultiplier: number = 0;
  @Input() centerZoneElement: HTMLElement | null = null;
  @Input() discardPileElement: HTMLElement | null = null;
  @Input() cardPlayed: ((cardId: string) => void) | null = null;
  @Input() isEntering: boolean = false;
  @Input() isLeaving: boolean = false;
  @Input() isDiscardHandLeaving: boolean = false;
  @Input() momentum: number = 0;
  @Input() daggers: number = 0;
  @Input() spirit: Spirit = Spirit.None;
  @Input() fire: boolean = false;
  @Input() water: boolean = false;
  @Input() earth: boolean = false;
  @Input() air: boolean = false;

  readonly dragStarted = output<void>();
  readonly dragEnded = output<void>();
  readonly cardPinned = output<void>();

  isDragging = false;
  isPinned = false;

  private readonly positionTransition = 'left 0.3s ease, top 0.3s ease, transform 0.3s ease';
  private readonly enterOffsetY = 180;
  private readonly innerCardSelector = '.card';
  private readonly discardHandFadeStartRatio = 0.72;

  private offsetX = 0;
  private offsetY = 0;
  private centerZoneRect: DOMRect | undefined;

  constructor(
    private readonly el: ElementRef,
    private readonly renderer: Renderer2,
    private readonly ngZone: NgZone,
  ) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['centerZoneElement']) {
      this.updateCenterZoneRect();
    }
    if (changes['cardIndex'] || changes['totalCards']) {
      this.renderer.setStyle(
        this.el.nativeElement,
        'transition',
        this.positionTransition,
      );
      this.resetPosition();
    }
    if (changes['isDiscardHandLeaving']?.currentValue === true) {
      this.flyToDiscardPileAnimation();
      return;
    }
    if (changes['isLeaving']?.currentValue === true) {
      this.fadeOutAnimation();
    }
  }

  ngAfterViewInit(): void {
    if (this.isEntering) {
      this.playEnterAnimation();
    } else {
      this.resetPosition();
    }
    this.resetZIndex();
    this.updateCenterZoneRect();
  }

  @HostListener('window:resize')
  onWindowResize(): void {
    this.resetPosition();
    this.updateCenterZoneRect();
  }

  private updateCenterZoneRect(): void {
    if (this.centerZoneElement) {
      this.centerZoneRect = this.centerZoneElement.getBoundingClientRect();
    }
  }

  resetPosition(): void {
    const { left, top, transform } = this.calculatePositionValues();
    this.renderer.setStyle(this.el.nativeElement, 'position', 'absolute');
    this.renderer.setStyle(this.el.nativeElement, 'left', left);
    this.renderer.setStyle(this.el.nativeElement, 'top', top);
    this.renderer.setStyle(this.el.nativeElement, 'transform', transform);
  }

  private calculatePositionValues(): { left: string; top: string; transform: string } {
    const containerRect = this.getContainerRect();
    const screenWidth = containerRect ? containerRect.width : 1600;
    const screenHeight = containerRect ? containerRect.height : 900;
    const centerX = screenWidth / 2;
    const centerY = screenHeight - 250;
    const horizontalSpacing = 90;
    const verticalSpacing = 10;
    const maxRotation = 10;

    const isEven = this.totalCards % 2 === 0;
    const centerIndex = isEven ? this.totalCards / 2 - 0.5 : Math.floor(this.totalCards / 2);

    const xOffset = (this.cardIndex - centerIndex) * horizontalSpacing;
    const yOffset = Math.abs(this.cardIndex - centerIndex) * verticalSpacing;

    let rotationAngle =
      this.totalCards === 1
        ? 0
        : (this.cardIndex - centerIndex) * (maxRotation / centerIndex);

    const cardRect = this.el.nativeElement.getBoundingClientRect();

    const left = `${centerX + xOffset - cardRect.width / 2 - 45}px`;
    const top = `${centerY - cardRect.height / 2 + yOffset}px`;
    const transform = `rotate(${rotationAngle}deg)`;

    return { left, top, transform };
  }

  private playEnterAnimation(): void {
    const { left, top, transform } = this.calculatePositionValues();
    const startTop = `${parseFloat(top) + this.enterOffsetY}px`;

    // Start at below-final position, invisible, no transition.
    this.renderer.setStyle(this.el.nativeElement, 'transition', 'none');
    this.renderer.setStyle(this.el.nativeElement, 'position', 'absolute');
    this.renderer.setStyle(this.el.nativeElement, 'left', left);
    this.renderer.setStyle(this.el.nativeElement, 'top', startTop);
    this.renderer.setStyle(this.el.nativeElement, 'transform', transform);
    this.renderer.setStyle(this.el.nativeElement, 'opacity', '0');

    // Force reflow so the browser paints the starting state before transitioning.
    this.el.nativeElement.getBoundingClientRect();

    // Run the transition to the final position outside Angular's zone so we don't
    // trigger unnecessary change-detection cycles for every animation frame.
    this.ngZone.runOutsideAngular(() => {
      requestAnimationFrame(() => {
        this.renderer.setStyle(
          this.el.nativeElement,
          'transition',
          `${this.positionTransition}, opacity 0.35s ease`,
        );
        this.renderer.setStyle(this.el.nativeElement, 'left', left);
        this.renderer.setStyle(this.el.nativeElement, 'top', top);
        this.renderer.setStyle(this.el.nativeElement, 'transform', transform);
        this.renderer.setStyle(this.el.nativeElement, 'opacity', '1');
      });
    });
  }

  onMouseDown(event: MouseEvent): void {
    if (this.isPinned) return;

    this.isDragging = true;
    this.dragStarted.emit();

    // Use the host element's viewport-relative corner for drag offsets.
    // getBoundingClientRect() on the 0×0 host still returns the correct top/left
    // because those reflect the element's position, not its size.
    // We must NOT use the inner .card rect here: at mousedown the card is in hover
    // state (transform: scale(1.2) translateY(-120px)), so its visual top is 120px
    // above the host's logical position — that would introduce a 120px jump.
    const hostRect = this.el.nativeElement.getBoundingClientRect();
    this.offsetX = event.clientX - hostRect.left;
    this.offsetY = event.clientY - hostRect.top;

    this.renderer.setStyle(this.el.nativeElement, 'transition', 'none');
    this.renderer.setStyle(this.el.nativeElement, 'transform', 'rotate(0deg)');
    this.bringToFront();

    // Refresh rect at drag start so it reflects the current layout
    this.updateCenterZoneRect();

    const cardEl = this.el.nativeElement.querySelector(this.innerCardSelector);
    if (cardEl) {
      this.renderer.addClass(cardEl, 'no-hover');
      this.renderer.addClass(cardEl, 'dragging');
    }

    event.preventDefault();
  }

  private getContainerRect(): DOMRect | null {
    const wrapper = this.el.nativeElement.closest('.encounter-wrapper');
    return wrapper ? (wrapper as HTMLElement).getBoundingClientRect() : null;
  }

  @HostListener('document:mousemove', ['$event'])
  onMouseMove(event: MouseEvent): void {
    if (!this.isDragging) return;

    const containerRect = this.getContainerRect();
    const containerLeft = containerRect ? containerRect.left : 0;
    const containerTop = containerRect ? containerRect.top : 0;

    this.renderer.setStyle(
      this.el.nativeElement,
      'left',
      `${event.clientX - this.offsetX - containerLeft}px`,
    );
    this.renderer.setStyle(
      this.el.nativeElement,
      'top',
      `${event.clientY - this.offsetY - containerTop}px`,
    );

    // Use the cursor position directly for zone detection. The inner card is
    // rendered 120 px above the cursor (translateY from .dragging), but the
    // cursor is the true drop-intent point and gives the most reliable results.
    if (this.isInCenterZone(event.clientX, event.clientY)) {
      this.applyGlowEffect();
    } else {
      this.removeGlowEffect();
    }
  }

  @HostListener('document:mouseup', ['$event'])
  onMouseUp(event: MouseEvent): void {
    if (!this.isDragging) return;

    this.isDragging = false;
    this.dragEnded.emit();
    this.removeGlowEffect();

    // Use the cursor position directly as the drop point.
    const cardCenterX = event.clientX;
    const cardCenterY = event.clientY;

    if (this.isInCenterZone(cardCenterX, cardCenterY)) {
      this.pinCardInCenterZone();
    } else {
      this.resetZIndex();
      this.renderer.setStyle(
        this.el.nativeElement,
        'transition',
        this.positionTransition,
      );
      this.resetPosition();

      const cardEl = this.el.nativeElement.querySelector(this.innerCardSelector);
      if (cardEl) {
        this.renderer.removeClass(cardEl, 'no-hover');
        this.renderer.removeClass(cardEl, 'dragging');
      }
    }
  }

  fadeOutAnimation(): void {
    this.renderer.setStyle(
      this.el.nativeElement,
      'transition',
      `${this.positionTransition}, opacity ${CARD_DISCARD_FADE_MS}ms ease`,
    );
    this.renderer.setStyle(this.el.nativeElement, 'opacity', '0');
    this.renderer.setStyle(this.el.nativeElement, 'pointer-events', 'none');
  }

  private flyToDiscardPileAnimation(): void {
    if (!this.discardPileElement) {
      this.fadeOutAnimation();
      return;
    }

    const containerRect = this.getContainerRect();
    if (!containerRect) {
      this.fadeOutAnimation();
      return;
    }

    const containerLeft = containerRect.left;
    const containerTop = containerRect.top;

    const hostRect = this.el.nativeElement.getBoundingClientRect();
    const discardPileRect = this.discardPileElement.getBoundingClientRect();
    const cardEl = this.el.nativeElement.querySelector(this.innerCardSelector) as HTMLElement | null;
    const cardRect = cardEl?.getBoundingClientRect();

    if (!cardRect) {
      this.fadeOutAnimation();
      return;
    }

    const hostToCardOffsetX = cardRect.left - hostRect.left;
    const hostToCardOffsetY = cardRect.top - hostRect.top;
    const discardPileCenterX = discardPileRect.left + discardPileRect.width / 2;
    const discardPileCenterY = discardPileRect.top + discardPileRect.height / 2;

    const targetLeft = discardPileCenterX - containerLeft - hostToCardOffsetX - cardRect.width / 2 + 50;
    const targetTop = discardPileCenterY - containerTop - hostToCardOffsetY - cardRect.height / 2 + 100;

    const currentLeft = (this.el.nativeElement as HTMLElement).style.left || `${hostRect.left - containerLeft}px`;
    const currentTop = (this.el.nativeElement as HTMLElement).style.top || `${hostRect.top - containerTop}px`;
    const currentTransform = (this.el.nativeElement as HTMLElement).style.transform || '';
    const finalTransform = currentTransform ? `${currentTransform} scale(0.35)` : 'scale(0.35)';
    const targetFrame = {
      left: `${targetLeft}px`,
      top: `${targetTop}px`,
      transform: finalTransform,
    };

    this.bringToFront();
    this.renderer.setStyle(this.el.nativeElement, 'pointer-events', 'none');

    const animation = this.el.nativeElement.animate(
      [
        {
          left: currentLeft,
          top: currentTop,
          transform: currentTransform,
          opacity: 1,
        },
        {
          ...targetFrame,
          opacity: 1,
          offset: this.discardHandFadeStartRatio,
        },
        {
          ...targetFrame,
          opacity: 0,
        },
      ],
      {
        duration: CARD_DISCARD_HAND_FLIGHT_MS,
        easing: 'cubic-bezier(0.2, 0.85, 0.25, 1)',
        fill: 'forwards',
      },
    );

    animation.onfinish = () => {
      this.renderer.setStyle(this.el.nativeElement, 'left', `${targetLeft}px`);
      this.renderer.setStyle(this.el.nativeElement, 'top', `${targetTop}px`);
      this.renderer.setStyle(this.el.nativeElement, 'transform', finalTransform);
      this.renderer.setStyle(this.el.nativeElement, 'opacity', '0');
    };
  }

  isInCenterZone(clientX: number, clientY: number): boolean {
    if (!this.centerZoneRect || !this.centerZoneElement) return false;
    return (
      clientX >= this.centerZoneRect.left &&
      clientX <= this.centerZoneRect.right &&
      clientY >= this.centerZoneRect.top &&
      clientY <= this.centerZoneRect.bottom
    );
  }

  private pinCardInCenterZone(): void {
    const shouldPinAndPlayCard =
      this.isMyTurn && this.playerEnergy >= this.manaCost && !!this.cardPlayed;
    if (!shouldPinAndPlayCard) {
      this.returnToHandPosition();
      return;
    }

    this.renderer.setStyle(this.el.nativeElement, 'opacity', '0');
    this.renderer.setStyle(this.el.nativeElement, 'pointer-events', 'none');
    this.cardPinned.emit();
    setTimeout(() => this.cardPlayed!(this.id), CARD_PLAY_DELAY_MS);
  }

  private returnToHandPosition(): void {
    this.resetZIndex();
    this.renderer.setStyle(
      this.el.nativeElement,
      'transition',
      this.positionTransition,
    );
    this.renderer.setStyle(this.el.nativeElement, 'opacity', '1');
    this.renderer.setStyle(this.el.nativeElement, 'pointer-events', 'auto');
    this.resetPosition();

    const cardEl = this.el.nativeElement.querySelector(this.innerCardSelector);
    if (cardEl) {
      this.renderer.removeClass(cardEl, 'no-hover');
      this.renderer.removeClass(cardEl, 'dragging');
    }
  }

  private bringToFront(): void {
    this.renderer.setStyle(this.el.nativeElement, 'z-index', 1000);
  }

  private resetZIndex(): void {
    this.renderer.setStyle(this.el.nativeElement, 'z-index', 20 + this.cardIndex);
  }

  private applyGlowEffect(): void {
    const cardEl = this.el.nativeElement.querySelector(this.innerCardSelector);
    if (cardEl && this.isMyTurn && this.playerEnergy >= this.manaCost) {
      this.renderer.setStyle(cardEl, 'box-shadow', `0 0 20px 8px ${this.borderColor}`);
    }
  }

  private removeGlowEffect(): void {
    const cardEl = this.el.nativeElement.querySelector(this.innerCardSelector);
    if (cardEl) {
      this.renderer.removeStyle(cardEl, 'box-shadow');
    }
  }
}
