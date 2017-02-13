<?xml version="1.0" encoding="UTF-8"?>
<tileset name="Ojects" tilewidth="448" tileheight="256" tilecount="42" columns="0">
 <tile id="0">
  <properties>
   <property name="opacity" type="float" value="0.5"/>
  </properties>
  <image width="165" height="99" source="../Unity/Assets/Sprites/Racing/PNG/Objects/arrow_white.png"/>
 </tile>
 <tile id="1">
  <image width="165" height="99" source="../Unity/Assets/Sprites/Racing/PNG/Objects/arrow_yellow.png"/>
 </tile>
 <tile id="2">
  <image width="56" height="56" source="../Unity/Assets/Sprites/Racing/PNG/Objects/barrel_blue.png"/>
  <objectgroup draworder="index">
   <object id="1" x="0" y="0" width="56" height="56">
    <ellipse/>
   </object>
  </objectgroup>
 </tile>
 <tile id="3">
  <image width="70" height="48" source="../Unity/Assets/Sprites/Racing/PNG/Objects/barrel_blue_down.png"/>
  <objectgroup draworder="index">
   <object id="2" x="0" y="0" width="70" height="48"/>
  </objectgroup>
 </tile>
 <tile id="4">
  <image width="56" height="56" source="../Unity/Assets/Sprites/Racing/PNG/Objects/barrel_red.png"/>
  <objectgroup draworder="index">
   <object id="3" x="0" y="0" width="56" height="56">
    <ellipse/>
   </object>
  </objectgroup>
 </tile>
 <tile id="5">
  <image width="70" height="48" source="../Unity/Assets/Sprites/Racing/PNG/Objects/barrel_red_down.png"/>
  <objectgroup draworder="index">
   <object id="1" x="0" y="0" width="70" height="48"/>
  </objectgroup>
 </tile>
 <tile id="6">
  <image width="210" height="62" source="../Unity/Assets/Sprites/Racing/PNG/Objects/barrier_red.png"/>
  <objectgroup draworder="index">
   <object id="1" x="0" y="0" width="210" height="62"/>
  </objectgroup>
 </tile>
 <tile id="7">
  <image width="210" height="62" source="../Unity/Assets/Sprites/Racing/PNG/Objects/barrier_red_race.png"/>
  <objectgroup draworder="index">
   <object id="2" x="0" y="0" width="210" height="62"/>
  </objectgroup>
 </tile>
 <tile id="8">
  <image width="210" height="62" source="../Unity/Assets/Sprites/Racing/PNG/Objects/barrier_white.png"/>
  <objectgroup draworder="index">
   <object id="1" x="0" y="0" width="210" height="62"/>
  </objectgroup>
 </tile>
 <tile id="9">
  <image width="210" height="62" source="../Unity/Assets/Sprites/Racing/PNG/Objects/barrier_white_race.png"/>
  <objectgroup draworder="index">
   <object id="1" x="0" y="0" width="210" height="62"/>
  </objectgroup>
 </tile>
 <tile id="10">
  <properties>
   <property name="AddComp" value="TrafficCone"/>
  </properties>
  <image width="46" height="44" source="../Unity/Assets/Sprites/Racing/PNG/Objects/cone_down.png"/>
  <objectgroup draworder="index">
   <object id="1" x="0.545455" y="5.45455" width="45.0909" height="31.8182"/>
  </objectgroup>
 </tile>
 <tile id="11">
  <properties>
   <property name="AddComp" value="TrafficCone"/>
  </properties>
  <image width="45" height="44" source="../Unity/Assets/Sprites/Racing/PNG/Objects/cone_straight.png"/>
  <objectgroup draworder="index">
   <object id="1" x="0" y="0" width="45" height="44"/>
  </objectgroup>
 </tile>
 <tile id="12">
  <image width="98" height="235" source="../Unity/Assets/Sprites/Racing/PNG/Objects/light_white.png"/>
 </tile>
 <tile id="13">
  <image width="98" height="235" source="../Unity/Assets/Sprites/Racing/PNG/Objects/light_yellow.png"/>
 </tile>
 <tile id="14">
  <image width="160" height="64" source="../Unity/Assets/Sprites/Racing/PNG/Objects/lights.png"/>
  <objectgroup draworder="index">
   <object id="1" x="0" y="13.6667" width="160" height="50.3333"/>
   <object id="3" x="64" y="0" width="30" height="30">
    <ellipse/>
   </object>
  </objectgroup>
 </tile>
 <tile id="15">
  <image width="109" height="95" source="../Unity/Assets/Sprites/Racing/PNG/Objects/oil.png"/>
 </tile>
 <tile id="16">
  <image width="89" height="72" source="../Unity/Assets/Sprites/Racing/PNG/Objects/rock1.png"/>
  <objectgroup draworder="index">
   <object id="3" x="1.63636" y="52.3636">
    <polygon points="0,0 14.1818,-29.0909 45.8182,-51.2727 84,-27.0909 68,12.7273 31.8182,6.72727 15.2727,18.5455"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="17">
  <image width="73" height="68" source="../Unity/Assets/Sprites/Racing/PNG/Objects/rock2.png"/>
  <objectgroup draworder="index">
   <object id="2" x="2.72727" y="32.7273">
    <polygon points="0,0 28.3636,-30.3636 68.9091,-20.3636 68.9091,10.9091 41.2727,34 8,27.8182"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="18">
  <image width="87" height="67" source="../Unity/Assets/Sprites/Racing/PNG/Objects/rock3.png"/>
  <objectgroup draworder="index">
   <object id="1" x="2" y="42">
    <polygon points="0,0 31.0909,-41.2727 66.5455,-35.6364 83.8182,-6.54545 54.9091,22.9091 17.8182,18.5455"/>
   </object>
  </objectgroup>
 </tile>
 <tile id="19">
  <image width="60" height="192" source="../Unity/Assets/Sprites/Racing/PNG/Objects/skidmark_long_1.png"/>
 </tile>
 <tile id="20">
  <image width="60" height="192" source="../Unity/Assets/Sprites/Racing/PNG/Objects/skidmark_long_2.png"/>
 </tile>
 <tile id="21">
  <image width="76" height="192" source="../Unity/Assets/Sprites/Racing/PNG/Objects/skidmark_long_3.png"/>
 </tile>
 <tile id="22">
  <image width="60" height="16" source="../Unity/Assets/Sprites/Racing/PNG/Objects/skidmark_short_1.png"/>
 </tile>
 <tile id="23">
  <image width="60" height="16" source="../Unity/Assets/Sprites/Racing/PNG/Objects/skidmark_short_2.png"/>
 </tile>
 <tile id="24">
  <image width="76" height="16" source="../Unity/Assets/Sprites/Racing/PNG/Objects/skidmark_short_3.png"/>
 </tile>
 <tile id="25">
  <image width="128" height="128" source="../Unity/Assets/Sprites/Racing/PNG/Objects/tent_blue.png"/>
 </tile>
 <tile id="26">
  <image width="256" height="256" source="../Unity/Assets/Sprites/Racing/PNG/Objects/tent_blue_large.png"/>
 </tile>
 <tile id="27">
  <image width="128" height="128" source="../Unity/Assets/Sprites/Racing/PNG/Objects/tent_red.png"/>
 </tile>
 <tile id="28">
  <image width="256" height="256" source="../Unity/Assets/Sprites/Racing/PNG/Objects/tent_red_large.png"/>
 </tile>
 <tile id="29">
  <image width="56" height="56" source="../Unity/Assets/Sprites/Racing/PNG/Objects/tires_red.png"/>
  <objectgroup draworder="index">
   <object id="1" x="0" y="0" width="56" height="56">
    <ellipse/>
   </object>
  </objectgroup>
 </tile>
 <tile id="30">
  <image width="56" height="56" source="../Unity/Assets/Sprites/Racing/PNG/Objects/tires_red_alt.png"/>
  <objectgroup draworder="index">
   <object id="2" x="0" y="0" width="56" height="56">
    <ellipse/>
   </object>
  </objectgroup>
 </tile>
 <tile id="31">
  <image width="56" height="56" source="../Unity/Assets/Sprites/Racing/PNG/Objects/tires_white.png"/>
  <objectgroup draworder="index">
   <object id="2" x="0" y="0" width="56" height="56">
    <ellipse/>
   </object>
  </objectgroup>
 </tile>
 <tile id="32">
  <image width="56" height="56" source="../Unity/Assets/Sprites/Racing/PNG/Objects/tires_white_alt.png"/>
  <objectgroup draworder="index">
   <object id="1" x="0" y="0" width="56" height="56">
    <ellipse/>
   </object>
  </objectgroup>
 </tile>
 <tile id="33">
  <image width="214" height="212" source="../Unity/Assets/Sprites/Racing/PNG/Objects/tree_large.png"/>
  <objectgroup draworder="index">
   <object id="2" x="73" y="73" width="66" height="66">
    <ellipse/>
   </object>
  </objectgroup>
 </tile>
 <tile id="34">
  <image width="141" height="141" source="../Unity/Assets/Sprites/Racing/PNG/Objects/tree_small.png"/>
  <objectgroup draworder="index">
   <object id="1" x="47.6667" y="47.6667" width="45.6667" height="45.6667">
    <ellipse/>
   </object>
  </objectgroup>
 </tile>
 <tile id="35">
  <image width="448" height="223" source="../Unity/Assets/Sprites/Racing/PNG/Objects/tribune_empty.png"/>
 </tile>
 <tile id="36">
  <image width="448" height="223" source="../Unity/Assets/Sprites/Racing/PNG/Objects/tribune_full.png"/>
  <objectgroup draworder="index">
   <object id="1" x="0" y="0" width="448" height="223"/>
  </objectgroup>
 </tile>
 <tile id="37">
  <image width="448" height="144" source="../Unity/Assets/Sprites/Racing/PNG/Objects/tribune_overhang_red.png"/>
  <objectgroup draworder="index">
   <object id="2" x="0" y="0" width="448" height="144"/>
  </objectgroup>
 </tile>
 <tile id="38">
  <image width="448" height="144" source="../Unity/Assets/Sprites/Racing/PNG/Objects/tribune_overhang_striped.png"/>
 </tile>
 <tile id="39">
  <image width="128" height="128" source="../Unity/Assets/Sprites/Racing/PNG/Objects/pit_label.png"/>
 </tile>
 <tile id="42">
  <properties>
   <property name="AddComp" value="StartPoint"/>
  </properties>
  <image width="128" height="128" source="../Unity/Assets/Sprites/Racing/PNG/Objects/start_right.png"/>
 </tile>
 <tile id="44">
  <properties>
   <property name="AddComp" value="TrackNavigation"/>
  </properties>
  <image width="50" height="50" source="../Unity/Assets/Sprites/Icons/PNG/White/1x/arrowRight.png"/>
 </tile>
</tileset>
