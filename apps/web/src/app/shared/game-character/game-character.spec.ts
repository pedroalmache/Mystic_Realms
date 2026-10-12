import { ComponentFixture, TestBed } from '@angular/core/testing';
import { GameCharacterComponent } from './game-character';
import { Spirit } from '../../core/services/single-player.service';
import { Assets } from '../../core/constants/assets.constants';

describe('GameCharacterComponent', () => {
  let component: GameCharacterComponent;
  let fixture: ComponentFixture<GameCharacterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GameCharacterComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(GameCharacterComponent);
    component = fixture.componentInstance;

    // Set required inputs
    fixture.componentRef.setInput('spriteUrl', 'test-sprite.png');

    fixture.detectChanges();
  });

  it('should display element icons when enabled', async () => {
    fixture.componentRef.setInput('fire', true);
    fixture.componentRef.setInput('water', true);
    fixture.detectChanges();

    const fireIcon = fixture.nativeElement.querySelector('img[alt="Fire"]');
    const waterIcon = fixture.nativeElement.querySelector('img[alt="Water"]');
    const earthIcon = fixture.nativeElement.querySelector('img[alt="Earth"]');
    const airIcon = fixture.nativeElement.querySelector('img[alt="Air"]');

    expect(fireIcon).toBeTruthy();
    expect(fireIcon.getAttribute('src')).toBe(Assets.Images.Characters.Elementalist.Powers.Fire);
    expect(waterIcon).toBeTruthy();
    expect(waterIcon.getAttribute('src')).toBe(Assets.Images.Characters.Elementalist.Powers.Water);
    expect(earthIcon).toBeFalsy();
    expect(airIcon).toBeFalsy();
  });

  it('should display all element icons when all are enabled', async () => {
    fixture.componentRef.setInput('fire', true);
    fixture.componentRef.setInput('water', true);
    fixture.componentRef.setInput('earth', true);
    fixture.componentRef.setInput('air', true);
    fixture.detectChanges();

    const icons = fixture.nativeElement.querySelectorAll('.element-icon');
    expect(icons.length).toBe(4);
  });
});
