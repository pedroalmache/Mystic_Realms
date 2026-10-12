/** Duration (ms) for the initiative-enter animation. */
export const ENTER_DURATION_MS = 350;
/** Stagger (ms) between consecutive initiative entries appearing. */
export const ENTER_STAGGER_MS = 150;
/** Duration (ms) for the initiative-leave animation. */
export const LEAVE_DURATION_MS = 600;

/** Stagger (ms) between consecutive new cards appearing in hand. */
export const CARD_ENTER_STAGGER_MS = 150;
/** Duration (ms) for a discarded card to fade out of the hand. */
export const CARD_DISCARD_FADE_MS = 600;
/** Duration (ms) for a DiscardHand card to fly into the discard pile. */
export const CARD_DISCARD_HAND_FLIGHT_MS = 900;

/** Duration (ms) of the discard pile pulse after a DiscardHand action. */
export const DISCARD_PILE_PULSE_MS = 700;
/** Duration (ms) for the armor update burst. */
export const ARMOR_BURST_MS = 1400;
/** Duration (ms) for the strength update burst. */
export const STRENGTH_BURST_MS = 1400;
/** Duration (ms) for the defense update burst. */
export const DEFENSE_BURST_MS = 1400;
/** Duration (ms) for the attack animation. */
export const ATTACK_ANIMATION_MS = 400;
/** Duration (ms) for the physical-damage defend animation and HP delta float. */
export const DAMAGE_ANIMATION_MS = 1500;
/** Duration (ms) for healing overlay and healing HP delta animations. */
export const HEAL_ANIMATION_MS = 2000;
/** Duration (ms) for poison overlay and poison delta animations. */
export const POISON_ANIMATION_MS = 2000;
/** Duration (ms) for mana overlay and mana delta animations. */
export const MANA_ANIMATION_MS = HEAL_ANIMATION_MS;

/** Delay (ms) between each incremental mana step. */
export const MANA_STEP_MS = 150;

/** Delay (ms) at the animation peak before swapping the turn number (turn-update animation totals 800 ms). */
export const TURN_UPDATE_PEAK_MS = 360;
/** Delay (ms) for the shrink-back tail wobble to finish (turn-update animation). */
export const TURN_UPDATE_TAIL_MS = 440;

/** Delay (ms) before notifying that a card has been played (card pin animation). */
export const CARD_PLAY_DELAY_MS = 300;
/** Duration (ms) for the Game Over blackout fade. */
export const GAME_OVER_FADE_MS = 1800;
/** Delay (ms) before the Game Over title begins fading in. */
export const GAME_OVER_TEXT_DELAY_MS = 700;
/** Duration (ms) for the Game Over title fade-in. */
export const GAME_OVER_TEXT_FADE_MS = 1200;
/** Remaining delay (ms) after the Game Over title starts so the blackout can finish. */
export const GAME_OVER_FINAL_DELAY_MS = GAME_OVER_FADE_MS - GAME_OVER_TEXT_DELAY_MS;
/** Delay (ms) after the Game Over title appears before the Continue button fades in. */
export const GAME_OVER_BUTTON_DELAY_MS = 2000;
/** Duration (ms) for the Continue button fade-in. */
export const GAME_OVER_BUTTON_FADE_MS = 800;

/** Duration (ms) for the Room Won blackout fade. */
export const ROOM_WON_FADE_MS = 1800;
/** Delay (ms) before the victory title begins fading in. */
export const ROOM_WON_TEXT_DELAY_MS = 700;
/** Duration (ms) for the victory title fade-in. */
export const ROOM_WON_TEXT_FADE_MS = 1200;
/** Overlap delay (ms) after the victory title starts so the blackout fade can finish. */
export const ROOM_WON_FINAL_DELAY_MS = ROOM_WON_FADE_MS - ROOM_WON_TEXT_DELAY_MS;
/** Delay (ms) after the victory title appears before transitioning to rewards. */
export const ROOM_WON_TRANSITION_DELAY_MS = 2000;
/** Delay (ms) before starting rewards intro blackout fade-out after mount. */
export const ROOM_REWARDS_INTRO_FADE_DELAY_MS = 50;

/** Delay (ms) before chest starts shaking (let the fade-in finish). */
export const CHEST_IDLE_DELAY_MS = 600;
/** Duration (ms) for each chest shake burst. */
export const CHEST_SHAKE_BURST_MS = 520;
/** Pause (ms) between chest shake bursts. */
export const CHEST_SHAKE_PAUSE_MS = 280;
/** Extra duration (ms) added to each successive shake burst to progressively lengthen the animation. */
export const CHEST_SHAKE_PROGRESSIVE_DELAY_MS = 180;
/** Duration (ms) of the flash burst that covers the closed→open chest transition. */
export const CHEST_FLASH_MS = 560;
/** Duration (ms) the open chest is shown before fading. */
export const CHEST_OPEN_HOLD_MS = 600;
/** Duration (ms) of the chest fade-out animation. */
export const CHEST_FADE_MS = 500;
/** Duration (ms) for each reward summary item rise/fade animation from the chest. */
export const CHEST_REWARD_POP_ITEM_DURATION_MS = 1200;
/** Stagger delay (ms) between consecutive reward summary item animations. */
export const CHEST_REWARD_POP_ITEM_STAGGER_MS = 1200;
/** Total duration (ms) of the reward summary items rising/fading from the chest. */
export const CHEST_REWARD_POP_TOTAL_MS = CHEST_REWARD_POP_ITEM_STAGGER_MS * 3;

/** Stagger delay (ms) between consecutive momentum icon fade-outs. */
export const MOMENTUM_REMOVE_STAGGER_MS = 150;
/** Duration (ms) for a momentum icon to fade out. */
export const MOMENTUM_REMOVE_FADE_MS = 300;

/** Duration (ms) for the elemental icon animations. */
export const ELEMENT_ANIMATION_MS = 600;
