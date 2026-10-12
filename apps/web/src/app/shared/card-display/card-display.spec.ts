import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CardDisplayComponent } from './card-display';
import { Assets } from '../../core/constants/assets.constants';
import { By } from '@angular/platform-browser';
import { Component, input } from '@angular/core';

describe('CardDisplayComponent', () => {
  let component: CardDisplayComponent;
  let fixture: ComponentFixture<CardDisplayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CardDisplayComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(CardDisplayComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should show permanent icon when permanent is true', () => {
    fixture.componentRef.setInput('permanent', true);
    fixture.detectChanges();
    const icon = fixture.debugElement.query(By.css('.permanent img'));
    expect(icon).toBeTruthy();
    expect(icon.nativeElement.src).toContain(Assets.Images.Cards.Display.Permanent);
  });

  it('should not show permanent icon when permanent is false', () => {
    fixture.componentRef.setInput('permanent', false);
    fixture.detectChanges();
    const icon = fixture.debugElement.query(By.css('.permanent img'));
    expect(icon).toBeFalsy();
  });
});
